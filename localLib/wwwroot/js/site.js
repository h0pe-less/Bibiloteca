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
    // Initialize arrays to track selections
    const selectedAuthors = [];
    const selectedCategories = [];

    // Load any existing selections (for validation failures)
    const initialAuthors = $('#selectedAuthorIds').val();
    const initialCategories = $('#selectedCategoryIds').val();

    if (initialAuthors) {
        initialAuthors.split(',').forEach(id => {
            if (id && !selectedAuthors.includes(id)) {
                selectedAuthors.push(id);
                const option = $(`#authorComboBox option[value="${id}"]`);
                if (option.length) {
                    addAuthorBadge(id, option.text());
                }
            }
        });
    }

    if (initialCategories) {
        initialCategories.split(',').forEach(id => {
            if (id && !selectedCategories.includes(id)) {
                selectedCategories.push(id);
                const option = $(`#categoryComboBox option[value="${id}"]`);
                if (option.length) {
                    addCategoryBadge(id, option.text());
                }
            }
        });
    }

    // Author functions
    $('#addAuthorBtn').click(function () {
        const comboBox = $('#authorComboBox');
        const authorId = comboBox.val();
        const authorName = comboBox.find('option:selected').text();

        if (authorId && !selectedAuthors.includes(authorId)) {
            selectedAuthors.push(authorId);
            addAuthorBadge(authorId, authorName);
            updateAuthorHiddenField();
        }
    });

    // Category functions
    $('#addCategoryBtn').click(function () {
        const comboBox = $('#categoryComboBox');
        const categoryId = comboBox.val();
        const categoryName = comboBox.find('option:selected').text();

        if (categoryId && !selectedCategories.includes(categoryId)) {
            selectedCategories.push(categoryId);
            addCategoryBadge(categoryId, categoryName);
            updateCategoryHiddenField();
        }
    });

    // Remove handlers
    $(document).on('click', '.selected-author .close', function () {
        const badge = $(this).closest('.selected-author');
        const authorId = badge.data('id');

        selectedAuthors.splice(selectedAuthors.indexOf(authorId), 1);
        badge.remove();
        updateAuthorHiddenField();
    });

    $(document).on('click', '.selected-category .close', function () {
        const badge = $(this).closest('.selected-category');
        const categoryId = badge.data('id');

        selectedCategories.splice(selectedCategories.indexOf(categoryId), 1);
        badge.remove();
        updateCategoryHiddenField();
    });

    // Helper functions
    function addAuthorBadge(id, name) {
        $('#selectedAuthorsContainer').append(`
            <div class="selected-author badge badge-primary mr-2 mb-2" data-id="${id}">
                ${name}
                <button type="button" class="close ml-2" aria-label="Remove">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        `);
    }

    function addCategoryBadge(id, name) {
        $('#selectedCategoriesContainer').append(`
            <div class="selected-category badge badge-secondary mr-2 mb-2" data-id="${id}">
                ${name}
                <button type="button" class="close ml-2" aria-label="Remove">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
        `);
    }

    function updateAuthorHiddenField() {
        $('#selectedAuthorIds').val(selectedAuthors.join(','));
    }

    function updateCategoryHiddenField() {
        $('#selectedCategoryIds').val(selectedCategories.join(','));
    }

    // Ensure fields are updated before form submission
    $('#bookForm').submit(function () {
        updateAuthorHiddenField();
        updateCategoryHiddenField();
        return true;
    });
});