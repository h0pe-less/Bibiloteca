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

// showing selected authors
$(document).ready(function () {
    const selectedAuthors = [];

    $('#addAuthorBtn').click(function () {
        const comboBox = $('#authorComboBox');
        const authorId = comboBox.val();
        const authorName = comboBox.find('option:selected').text();

        if (authorId && !selectedAuthors.includes(authorId)) {
            selectedAuthors.push(authorId);

            // Add to display
            $('#selectedAuthorsContainer').append(`
                <div class="selected-author badge badge-primary mr-2 mb-2" data-id="${authorId}">
                    ${authorName}
                    <button type="button" class="close ml-2" aria-label="Remove">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            `);

            // Update hidden field
            $('#selectedAuthors').val(selectedAuthors.join(','));
        }
    });

    // Remove author when X is clicked
    $(document).on('click', '.selected-author .close', function () {
        const badge = $(this).closest('.selected-author');
        const authorId = badge.data('id');

        selectedAuthors.splice(selectedAuthors.indexOf(authorId), 1);
        badge.remove();
        $('#selectedAuthors').val(selectedAuthors.join(','));
    });
});

$(document).ready(function () {
    const selectedCategories = [];

    $('#addCategoryBtn').click(function () {
        const comboBox = $('#categoryComboBox');
        const categoryId = comboBox.val();
        const categoryName = comboBox.find('option:selected').text();

        if (categoryId && !selectedCategories.includes(categoryId)) {
            selectedCategories.push(categoryId);

            // Add to display
            $('#selectedCategoriesContainer').append(`
                <div class="selected-category badge badge-primary mr-2 mb-2" data-id="${categoryId}">
                    ${categoryName}
                    <button type="button" class="close ml-2" aria-label="Remove">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
            `);

            // Update hidden field
            $('#selectedCategories').val(selectedCategories.join(','));
        }
    });

    // Remove author when X is clicked
    $(document).on('click', '.selected-category .close', function () {
        const badge = $(this).closest('.selected-category');
        const categoryId = badge.data('id');

        selectedCategories.splice(selectedCategories.indexOf(categoryId), 1);
        badge.remove();
        $('#selectedCategories').val(selectedCategories.join(','));
    });
});