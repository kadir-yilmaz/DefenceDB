document.addEventListener("DOMContentLoaded", function () {
    // === Expandable Search Bar (Icon → Expand Left) ===
    const searchInput = document.getElementById('searchInput');
    const searchContainer = document.getElementById('searchContainer');
    const searchSuggestions = document.getElementById('searchSuggestions');
    const searchForm = document.getElementById('searchForm');
    const searchToggleBtn = document.getElementById('searchToggleBtn');

    if (searchInput && searchForm && searchToggleBtn) {
        // Toggle search open/close
        searchToggleBtn.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            if (searchForm.classList.contains('expanded')) {
                // If it has text, submit search. Otherwise, close it.
                if (searchInput.value.trim().length > 0) {
                    searchForm.submit();
                } else {
                    closeSearch();
                }
            } else {
                // Open search
                searchForm.classList.add('expanded');
                setTimeout(() => {
                    searchInput.focus();
                }, 100);
            }
        });

        // Close search helper
        function closeSearch() {
            searchForm.classList.remove('expanded');
            searchInput.value = '';
            if (searchSuggestions) {
                searchSuggestions.classList.add('d-none');
            }
        }

        // Close on click outside
        document.addEventListener('click', function (e) {
            if (searchForm.classList.contains('expanded') &&
                !searchContainer.contains(e.target)) {
                closeSearch();
            }
        });

        // Close on Escape key
        searchInput.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') {
                closeSearch();
                searchToggleBtn.focus();
            }
        });

        // Submit form on Enter (allow normal form submission)
        searchInput.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && searchInput.value.trim()) {
                searchForm.submit();
            }
        });
    }

    // === Mobile Categories Touch/Double-Tap Logic ===
    const isMobile = () => window.innerWidth <= 991.98;

    document.querySelectorAll('.cat-item-level1, .cat-item-level2').forEach(item => {
        const link = item.querySelector(':scope > a');
        const hasSubMenu = item.querySelector('.categories-level2-bar, .categories-level3-dropdown');

        if (link && hasSubMenu) {
            link.addEventListener('click', function (e) {
                if (isMobile()) {
                    if (!item.classList.contains('mobile-active')) {
                        // Prevent navigation, expand instead
                        e.preventDefault();
                        e.stopPropagation();

                        // Close siblings at the same level
                        const siblings = item.parentElement.querySelectorAll(':scope > .mobile-active');
                        siblings.forEach(s => s.classList.remove('mobile-active'));

                        // Open this item
                        item.classList.add('mobile-active');
                    }
                    // If it already has 'mobile-active', we let the click pass through (navigate)
                }
            });
        }
    });

    // Close menus if clicking outside on mobile
    document.addEventListener('click', function(e) {
        if (isMobile()) {
            if (!e.target.closest('.categories-nav-wrapper')) {
                document.querySelectorAll('.mobile-active').forEach(activeItem => {
                    activeItem.classList.remove('mobile-active');
                });
            }
        }
    });

    // Auto-scroll to center the active category on mobile on page load
    if (isMobile()) {
        const scrollContainers = document.querySelectorAll('.categories-level1, .categories-level2-bar ul');
        scrollContainers.forEach(container => {
            const activeItem = container.querySelector('.mobile-active-text')?.closest('li');
            if (activeItem) {
                // Wait a tiny bit for layout to settle
                setTimeout(() => {
                    const containerWidth = container.offsetWidth;
                    const itemOffset = activeItem.offsetLeft;
                    const itemWidth = activeItem.offsetWidth;
                    // Calculate scroll position to center the item
                    container.scrollTo({
                        left: itemOffset - (containerWidth / 2) + (itemWidth / 2),
                        behavior: 'smooth'
                    });
                }, 100);
            }
        });
    }
});