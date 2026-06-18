document.addEventListener("DOMContentLoaded", function () {
    // === Mega Menu ===
    const triggerBtn = document.getElementById('categoriesTriggerBtn');
    const megaMenu = document.getElementById('megaMenu');
    const sidebar = document.getElementById('megaMenuSidebar');
    const content = document.getElementById('megaMenuContent');

    // Eğer elementlerden biri sayfada yoksa kodu durdur (hata fırlatmasını engeller)
    if (!triggerBtn || !megaMenu || !sidebar || !content) {
        console.warn("DefenceDB Mega Menu hazır değil veya HTML eksik.");
        return;
    }

    const sidebarItems = sidebar.querySelectorAll('.mega-menu-sidebar-item');
    const contentPanels = content.querySelectorAll('.mega-menu-content-panel');
    let isOpen = false;
    let activeCategoryId = null;
    let hoverTimeout;

    function showPanel(catId) {
        contentPanels.forEach(p => {
            p.classList.toggle('active', p.dataset.categoryId == catId);
        });
        sidebarItems.forEach(item => {
            item.classList.toggle('active', item.dataset.categoryId == catId);
        });
    }

    function openMenu() {
        megaMenu.classList.add('open');
        triggerBtn.classList.add('active');
        isOpen = true;

        if (!activeCategoryId && sidebarItems.length > 0) {
            const urlParams = new URLSearchParams(window.location.search);
            const currentSlug = urlParams.get('categorySlug');
            let matchedId = null;

            if (currentSlug) {
                sidebarItems.forEach(item => {
                    const href = item.getAttribute('href');
                    if (href && href.includes(currentSlug)) {
                        matchedId = item.dataset.categoryId;
                    }
                });
            }
            activeCategoryId = matchedId ? matchedId : sidebarItems[0].dataset.categoryId;
            showPanel(activeCategoryId);
        }
    }

    function closeMenu() {
        megaMenu.classList.remove('open');
        triggerBtn.classList.remove('active');
        isOpen = false;
    }

    // --- PC İÇİN HOVER MANTIĞI ---
    const handleMouseEnter = () => {
        if (window.innerWidth >= 992) {
            clearTimeout(hoverTimeout);
            if (!isOpen) openMenu();
        }
    };

    const handleMouseLeave = () => {
        if (window.innerWidth >= 992) {
            hoverTimeout = setTimeout(() => {
                closeMenu();
            }, 100);
        }
    };

    triggerBtn.addEventListener('mouseenter', handleMouseEnter);
    triggerBtn.addEventListener('mouseleave', handleMouseLeave);
    megaMenu.addEventListener('mouseenter', handleMouseEnter);
    megaMenu.addEventListener('mouseleave', handleMouseLeave);

    // --- MOBİL İÇİN TIKLAMA MANTIĞI ---
    triggerBtn.addEventListener('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
        if (isOpen) closeMenu();
        else openMenu();
    });

    sidebarItems.forEach(item => {
        item.addEventListener('mouseenter', function () {
            if (window.innerWidth >= 992) {
                activeCategoryId = this.dataset.categoryId;
                showPanel(activeCategoryId);
            }
        });

        item.addEventListener('click', function (e) {
            if (window.innerWidth < 992) {
                const clickedId = this.dataset.categoryId;
                if (activeCategoryId !== clickedId) {
                    e.preventDefault();
                    activeCategoryId = clickedId;
                    showPanel(activeCategoryId);
                } else {
                    closeMenu();
                }
            } else {
                closeMenu();
            }
        });
    });

    document.addEventListener('click', function (e) {
        if (isOpen && !megaMenu.contains(e.target) && !triggerBtn.contains(e.target)) {
            closeMenu();
        }
    });

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape' && isOpen) {
            closeMenu();
        }
    });
});