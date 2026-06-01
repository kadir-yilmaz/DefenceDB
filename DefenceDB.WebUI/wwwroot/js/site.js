// Dynamic Theme & AJAX Popup Engine & Radar Systems Loader for DefenceDB



$(document).ready(function () {
    // ----------------------------------------------------
    // Theme is permanently light now. Theme switcher removed.
    // ----------------------------------------------------
    $('html').attr('data-theme', 'light');

    // ----------------------------------------------------
    // 2. AJAX Popup Engine & Radar Systems Loader
    // ----------------------------------------------------
    
    // Handler for Aircraft Engine Modal triggers
    $(document).on('click', '.open-engine-modal', function (e) {
        e.preventDefault();
        var engineId = $(this).data('id');
        if (!engineId) return;

        showLoader();

        $.ajax({
            url: '/Product/GetEngineDetails',
            type: 'GET',
            data: { id: engineId },
            success: function (data) {
                $('#modalContentPlaceholder').html(data);
                var myModal = new bootstrap.Modal(document.getElementById('detailModal'));
                myModal.show();
            },
            error: function (xhr, status, error) {
                console.error("Engine Load Error:", error);
                showError("Motor detayları yüklenemedi. Lütfen daha sonra tekrar deneyiniz.");
                var myModal = new bootstrap.Modal(document.getElementById('detailModal'));
                myModal.show();
            }
        });
    });

    // Handler for Radar System Modal triggers
    $(document).on('click', '.open-radar-modal', function (e) {
        e.preventDefault();
        var radarId = $(this).data('id');
        if (!radarId) return;

        showLoader();

        $.ajax({
            url: '/Product/GetRadarDetails',
            type: 'GET',
            data: { id: radarId },
            success: function (data) {
                $('#modalContentPlaceholder').html(data);
                var myModal = new bootstrap.Modal(document.getElementById('detailModal'));
                myModal.show();
            },
            error: function (xhr, status, error) {
                console.error("Radar Load Error:", error);
                showError("Radar detayları yüklenemedi. Lütfen daha sonra tekrar deneyiniz.");
                var myModal = new bootstrap.Modal(document.getElementById('detailModal'));
                myModal.show();
            }
        });
    });

    // Helper to show a loading state in the placeholder
    function showLoader() {
        var loaderHtml = `
            <div class="modal-body-theme text-center p-5 text-primary-theme rounded-4">
                <div class="spinner-border text-warning" role="status">
                    <span class="visually-hidden">Yükleniyor...</span>
                </div>
                <p class="mt-3 text-secondary-theme">Teknik veriler güvenli kanaldan sorgulanıyor...</p>
            </div>
        `;
        $('#modalContentPlaceholder').html(loaderHtml);
    }

    // Helper to show error messages
    function showError(message) {
        var errorHtml = `
            <div class="modal-header-theme text-danger d-flex justify-content-between align-items-center">
                <h5 class="modal-title"><i class="bi bi-exclamation-triangle-fill"></i> Sistem Hatası</h5>
                <button type="button" class="btn-close btn-close-theme" data-bs-dismiss="modal" aria-label="Kapat"></button>
            </div>
            <div class="modal-body-theme text-primary-theme">
                <p>${message}</p>
            </div>
            <div class="modal-footer-theme d-flex justify-content-end gap-2">
                <button type="button" class="btn btn-outline-warning btn-sm rounded-pill px-3" data-bs-dismiss="modal">Kapat</button>
            </div>
        `;
        $('#modalContentPlaceholder').html(errorHtml);
    }

    // ----------------------------------------------------
    // 3. Instant Search Autocomplete
    // ----------------------------------------------------
    let searchTimeout;
    $('#searchInput').on('input', function() {
        clearTimeout(searchTimeout);
        let query = $(this).val().trim();
        let suggestionsBox = $('#searchSuggestions');
        let suggestionsList = $('#searchSuggestionsList');

        if (query.length < 2) {
            suggestionsBox.addClass('d-none');
            return;
        }

        searchTimeout = setTimeout(() => {
            $.ajax({
                url: '/Product/SearchSuggestions',
                type: 'GET',
                data: { term: query },
                success: function(data) {
                    suggestionsList.empty();
                    if (data && data.length > 0) {
                        data.forEach(item => {
                            let flag = item.flagUrl ? `<img src="${item.flagUrl}" alt="${item.country}" class="rounded-1 shadow-sm border border-secondary" style="width: 26px; height: 18px; object-fit: cover;">` : '';
                            let cat = item.categoryName ? `<span class="badge bg-light text-secondary-theme border fw-medium rounded-pill px-3 py-2" style="font-size: 0.75rem;">${item.categoryName}</span>` : '';
                            
                            let html = `
                                <a href="/Product/Detail/${item.id}-${item.slug}" class="list-group-item list-group-item-action bg-transparent border-bottom border-theme d-flex align-items-center justify-content-between p-3" style="transition: background-color 0.2s ease;">
                                    <div class="d-flex align-items-center gap-3 flex-grow-1 overflow-hidden">
                                        <img src="${item.image}" alt="${item.name}" onerror="handleImageError(this, '${item.name.replace(/'/g, "\\'")}')" class="rounded shadow-sm" style="width: 50px; height: 50px; object-fit: cover; border: 1px solid rgba(0,0,0,0.1);">
                                        <h6 class="mb-0 text-primary-theme text-truncate" style="font-size: 1.05rem; font-weight: 600;">${item.name}</h6>
                                    </div>
                                    <div class="d-flex align-items-center gap-3 ms-3 flex-shrink-0">
                                        ${cat}
                                        ${flag}
                                    </div>
                                </a>
                            `;
                            suggestionsList.append(html);
                        });
                        suggestionsBox.removeClass('d-none');
                    } else {
                        suggestionsList.html('<div class="p-3 text-center text-secondary-theme fs-8">Sonuç bulunamadı.</div>');
                        suggestionsBox.removeClass('d-none');
                    }
                }
            });
        }, 300); // 300ms debounce
    });

    // Hide suggestions when clicking outside
    $(document).on('click', function(e) {
        if (!$(e.target).closest('#searchContainer').length) {
            $('#searchSuggestions').addClass('d-none');
        }
    });

    // Show suggestions again if input is focused and has text
    $('#searchInput').on('focus', function() {
        if ($(this).val().trim().length >= 2 && $('#searchSuggestionsList').children().length > 0) {
            $('#searchSuggestions').removeClass('d-none');
        }
    });
});

// Global Product click handler
window.handleProductClick = function(event, url) {
    if (event) {
        // allow middle click or ctrl+click to open in new tab normally if it's on an anchor
        // but since this is on a div, we just navigate
        var isNewTab = event.ctrlKey || event.shiftKey || event.metaKey || (event.button && event.button === 1);
        if (isNewTab) {
            window.open(url, '_blank');
            return;
        }
        event.preventDefault();
        event.stopPropagation();
    }
    
    window.location.href = url;
};
