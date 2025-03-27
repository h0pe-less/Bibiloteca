// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function animateCounter(elementId, targetNumber) {
    const counter = document.getElementById(elementId);
    const duration = 2000;
    const startTime = performance.now();

    function updateCounter(currentTime) {
        const elapsedTime = currentTime - startTime;
        const progress = Math.min(elapsedTime / duration, 1);
        const easeProgress = easeOutQuad(progress);
        const currentNumber = Math.floor(easeProgress * targetNumber);

        counter.textContent = currentNumber.toLocaleString();

        if (progress < 1) {
            requestAnimationFrame(updateCounter);
        } else {
            counter.textContent = targetNumber.toLocaleString();
        }
    }

    function easeOutQuad(t) {
        return t * (2 - t);
    }

    requestAnimationFrame(updateCounter);
}

window.addEventListener('DOMContentLoaded', () => {
    animateCounter('books-counter', 1234);
    animateCounter('authors-counter', 657);
    animateCounter('topics-counter', 89);
});
