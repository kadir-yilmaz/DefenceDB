document.addEventListener("DOMContentLoaded", function () {
    // === Mobile / Desktop Search Bar Expanding & Autocomplete Focus ===
    const searchInput = document.getElementById('searchInput');
    const searchContainer = document.getElementById('searchContainer');
    const searchSuggestions = document.getElementById('searchSuggestions');
    const mobileSearchTrigger = document.getElementById('mobileSearchTrigger');
    const mobileSearchClose = document.getElementById('mobileSearchClose');

    if (searchInput && searchContainer) {
        // Desktop focus scaling
        searchInput.addEventListener('focus', function () {
            if (window.innerWidth >= 992) {
                searchInput.style.width = '450px';
            } else if (window.innerWidth >= 768) {
                searchInput.style.width = '350px';
            }
        });

        // Desktop blur scaling
        searchInput.addEventListener('blur', function () {
            if (window.innerWidth >= 768) {
                setTimeout(() => {
                    if (searchSuggestions && !searchSuggestions.matches(':hover')) {
                        searchInput.style.width = '240px';
                    }
                }, 200);
            }
        });

        // Mobile click trigger to expand search
        if (mobileSearchTrigger) {
            mobileSearchTrigger.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                searchContainer.classList.add('expanded');
                // Allow CSS transition to kick in before focus
                setTimeout(() => {
                    searchInput.focus();
                }, 50);
            });
        }

        // Mobile close button to collapse search
        if (mobileSearchClose) {
            mobileSearchClose.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                searchContainer.classList.remove('expanded');
                searchInput.value = '';
                searchInput.style.width = '';
                if (searchSuggestions) {
                    searchSuggestions.classList.add('d-none');
                }
            });
        }

        // Collapse search on mobile if clicked outside
        document.addEventListener('click', function (e) {
            if (!searchContainer.contains(e.target) && !e.target.closest('#mobileSearchTrigger')) {
                if (window.innerWidth < 992) {
                    searchContainer.classList.remove('expanded');
                    searchInput.style.width = '';
                }
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