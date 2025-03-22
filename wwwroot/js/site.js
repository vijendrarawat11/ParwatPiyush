// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const ticker = document.querySelector(".news-ticker ul"); 
    const items = Array.from(ticker.children); 
    let totalWidth = 0;
    items.forEach((item) => {
        totalWidth += item.offsetWidth + 50; 
    });

    // Set the width of <ul> to be at least twice its original width
    ticker.style.width = totalWidth * 2 + "px";

    // Duplicate items only if needed (prevents excessive duplication)
    while (ticker.scrollWidth < ticker.parentElement.clientWidth * 2) {
        items.forEach((item) => {
            let clone = item.cloneNode(true);
            ticker.appendChild(clone);
        });
    }


    let tickerPosition = 0;
    function moveTicker() {
        tickerPosition -= 1; 
        if (Math.abs(tickerPosition) >= totalWidth / 2) {
            tickerPosition = 0; 
        }
        ticker.style.transform = `translateX(${tickerPosition}px)`;
        requestAnimationFrame(moveTicker);
    }

    moveTicker(); 
});
