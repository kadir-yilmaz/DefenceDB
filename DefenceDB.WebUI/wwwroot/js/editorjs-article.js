/**
 * Editor.js Article Editor — Initialization & Form Integration
 * Handles editor setup, data loading, form submission, and stats tracking
 */
(function () {
    'use strict';

    // ── State ──
    var editor = null;
    var hasChanges = false;
    var initialData = null;

    // ── DOM References ──
    var editorHolder = document.getElementById('editorjs');
    var hiddenInput = document.getElementById('ContentMarkdownHidden');
    var form = document.getElementById('articleEditorForm');
    var titleInput = document.getElementById('articleTitleInput');
    var statusEl = document.getElementById('editorSaveStatus');
    var wordCountEl = document.getElementById('editorWordCount');
    var blockCountEl = document.getElementById('editorBlockCount');
    var legacyWarning = document.getElementById('editorLegacyWarning');

    if (!editorHolder || !hiddenInput || !form) {
        console.warn('Editor.js: Required DOM elements not found');
        return;
    }

    // ── Parse existing data ──
    var rawContent = hiddenInput.value || '';
    var isLegacyMarkdown = false;

    if (rawContent.trim()) {
        try {
            var parsed = JSON.parse(rawContent);
            if (parsed && parsed.blocks) {
                initialData = parsed;
            } else {
                isLegacyMarkdown = true;
            }
        } catch (e) {
            // Not JSON — treat as legacy Markdown
            isLegacyMarkdown = true;
        }
    }

    // Show legacy warning if needed
    if (isLegacyMarkdown && legacyWarning) {
        legacyWarning.style.display = 'flex';
    }

    // ── Upload image endpoint ──
    var uploadEndpoint = editorHolder.getAttribute('data-upload-url') || '';

    // ── Safely resolve tool classes from window globals ──
    var HeaderTool = window.Header;
    var ListTool = window.EditorjsList || window.List || window.NestedList;
    var QuoteTool = window.Quote;
    var DelimiterTool = window.Delimiter;
    var TableTool = window.Table;
    var CodeBlockTool = window.CodeTool;
    var WarningTool = window.Warning;
    var RawHtmlTool = window.RawTool;
    var EmbedTool = window.Embed;
    var ImageBlockTool = window.ImageTool;
    var UnderlineTool = window.Underline;

    // ── Configure tools (only include tools that loaded) ──
    var tools = {};

    if (HeaderTool) {
        tools.header = {
            class: HeaderTool,
            inlineToolbar: true,
            config: {
                placeholder: 'Başlık girin...',
                levels: [2, 3, 4],
                defaultLevel: 2
            }
        };
    }

    if (ListTool) {
        tools.list = {
            class: ListTool,
            inlineToolbar: true,
            config: {
                defaultStyle: 'unordered'
            }
        };
    }

    if (QuoteTool) {
        tools.quote = {
            class: QuoteTool,
            inlineToolbar: true,
            config: {
                quotePlaceholder: 'Alıntı yazın...',
                captionPlaceholder: 'Alıntı kaynağı'
            }
        };
    }

    if (DelimiterTool) {
        tools.delimiter = DelimiterTool;
    }

    if (TableTool) {
        tools.table = {
            class: TableTool,
            inlineToolbar: true,
            config: {
                rows: 3,
                cols: 3
            }
        };
    }

    if (CodeBlockTool) {
        tools.code = CodeBlockTool;
    }

    if (WarningTool) {
        tools.warning = {
            class: WarningTool,
            inlineToolbar: false,
            config: {
                titlePlaceholder: 'Başlık',
                messagePlaceholder: 'Mesaj'
            }
        };
    }

    if (RawHtmlTool) {
        tools.raw = RawHtmlTool;
    }

    if (EmbedTool) {
        tools.embed = {
            class: EmbedTool,
            config: {
                services: {
                    youtube: true,
                    vimeo: true
                }
            }
        };
    }

    if (UnderlineTool) {
        tools.underline = UnderlineTool;
    }

    // Add image tool only if upload endpoint and tool exist
    if (uploadEndpoint && ImageBlockTool) {
        tools.image = {
            class: ImageBlockTool,
            config: {
                endpoints: {
                    byFile: uploadEndpoint
                },
                field: 'image',
                types: 'image/jpeg, image/png, image/gif, image/webp, image/svg+xml',
                captionPlaceholder: 'Görsel açıklaması...',
                buttonContent: 'Görsel Yükle'
            }
        };
    }

    // ── Initialize Editor.js ──
    editor = new EditorJS({
        holder: 'editorjs',
        tools: tools,
        data: initialData || { blocks: [] },
        placeholder: 'Yazmaya başlayın veya "+" ile blok ekleyin...',
        autofocus: !initialData,
        onChange: function () {
            hasChanges = true;
            updateStatus();
            debouncedUpdateStats();
        },
        onReady: function () {
            updateStats();
            updateStatus();
            initTableDragAndDrop();
            console.log('Editor.js initialized successfully');
        },
        i18n: {
            messages: {
                ui: {
                    blockTunes: {
                        toggler: {
                            'Click to tune': 'Ayarla',
                            'or drag to move': 'veya sürükle'
                        }
                    },
                    inlineToolbar: {
                        converter: {
                            'Convert to': 'Dönüştür'
                        }
                    },
                    toolbar: {
                        toolbox: {
                            Add: 'Ekle'
                        }
                    }
                },
                toolNames: {
                    Text: 'Metin',
                    Heading: 'Başlık',
                    List: 'Liste',
                    Quote: 'Alıntı',
                    Delimiter: 'Ayırıcı',
                    Table: 'Tablo',
                    Code: 'Kod',
                    Warning: 'Uyarı',
                    Image: 'Görsel',
                    Embed: 'Gömme',
                    'Raw HTML': 'Ham HTML',
                    Bold: 'Kalın',
                    Italic: 'İtalik',
                    Underline: 'Altı Çizili',
                    Link: 'Bağlantı'
                },
                tools: {
                    header: {
                        'Heading 2': 'Başlık 2',
                        'Heading 3': 'Başlık 3',
                        'Heading 4': 'Başlık 4'
                    },
                    list: {
                        Unordered: 'Sırasız',
                        Ordered: 'Sıralı'
                    },
                    warning: {
                        Title: 'Başlık',
                        Message: 'Mesaj'
                    },
                    image: {
                        Caption: 'Açıklama',
                        'Select an Image': 'Görsel Seç',
                        'With border': 'Kenarlıklı',
                        'Stretch image': 'Genişlet',
                        'With background': 'Arka planlı'
                    },
                    table: {
                        'With headings': 'Başlıklı',
                        'Without headings': 'Başlıksız',
                        'Add row above': 'Üste satır ekle',
                        'Add row below': 'Alta satır ekle',
                        'Delete row': 'Satır sil',
                        'Add column to left': 'Sola sütun ekle',
                        'Add column to right': 'Sağa sütun ekle',
                        'Delete column': 'Sütun sil'
                    }
                },
                blockTunes: {
                    delete: { Delete: 'Sil', 'Click to delete': 'Silmek için tıkla' },
                    moveUp: { 'Move up': 'Yukarı taşı' },
                    moveDown: { 'Move down': 'Aşağı taşı' }
                }
            }
        }
    });

    // ── Form Submission ──
    form.addEventListener('submit', async function (e) {
        e.preventDefault();

        // Validate title
        if (!titleInput.value.trim()) {
            titleInput.classList.add('is-invalid');
            titleInput.focus();
            titleInput.scrollIntoView({ behavior: 'smooth', block: 'center' });
            return;
        }

        // Validate category
        var categorySelect = document.getElementById('articleCategorySelect');
        if (categorySelect && !categorySelect.value) {
            categorySelect.classList.add('is-invalid');
            categorySelect.scrollIntoView({ behavior: 'smooth', block: 'center' });
            categorySelect.focus();
            return;
        }

        try {
            var outputData = await editor.save();

            // Validate content — at least one non-empty block
            var hasContent = outputData.blocks.some(function (block) {
                if (block.type === 'delimiter') return true;
                if (block.data && block.data.text && block.data.text.trim()) return true;
                if (block.data && block.data.items && block.data.items.length > 0) return true;
                if (block.data && block.data.code && block.data.code.trim()) return true;
                if (block.data && block.data.html && block.data.html.trim()) return true;
                if (block.data && block.data.content && block.data.content.length > 0) return true;
                if (block.data && block.data.file && block.data.file.url) return true;
                return false;
            });

            if (!hasContent) {
                editorHolder.scrollIntoView({ behavior: 'smooth', block: 'center' });
                return;
            }

            // Write JSON to hidden input
            hiddenInput.value = JSON.stringify(outputData);
            hasChanges = false;
            updateStatus();

            // Submit the form natively (form.submit() bypasses event listeners)
            form.submit();
        } catch (error) {
            console.error('Editor.js save error:', error);
        }
    });

    // ── Title clear error ──
    if (titleInput) {
        titleInput.addEventListener('input', function () {
            this.classList.remove('is-invalid');
            hasChanges = true;
            updateStatus();
        });
    }

    // ── Category clear error ──
    var catSelect = document.getElementById('articleCategorySelect');
    if (catSelect) {
        catSelect.addEventListener('change', function () {
            this.classList.remove('is-invalid');
        });
    }

    // ── Status Indicator ──
    function updateStatus() {
        if (!statusEl) return;
        if (hasChanges) {
            statusEl.classList.add('has-changes');
            statusEl.innerHTML = '<i class="bi bi-circle-fill"></i> Değişiklikler var';
        } else {
            statusEl.classList.remove('has-changes');
            statusEl.innerHTML = '<i class="bi bi-check-circle-fill"></i> Hazır';
        }
    }

    // ── Debounced stats update ──
    var statsTimer = null;
    function debouncedUpdateStats() {
        clearTimeout(statsTimer);
        statsTimer = setTimeout(updateStats, 500);
    }

    // ── Stats (word/block count) ──
    function updateStats() {
        if (!editor || !editor.save) return;

        editor.save().then(function (data) {
            var totalWords = 0;
            var blockCount = data.blocks.length;

            data.blocks.forEach(function (block) {
                var text = '';
                if (block.data && block.data.text) text = block.data.text;
                if (block.data && block.data.code) text = block.data.code;
                if (block.data && block.data.items) {
                    block.data.items.forEach(function (item) {
                        if (typeof item === 'string') text += ' ' + item;
                        else if (item && item.content) text += ' ' + item.content;
                    });
                }
                // Strip HTML tags for word counting
                var clean = text.replace(/<[^>]*>/g, '').trim();
                if (clean) {
                    totalWords += clean.split(/\s+/).length;
                }
            });

            if (wordCountEl) wordCountEl.textContent = totalWords;
            if (blockCountEl) blockCountEl.textContent = blockCount;
        }).catch(function () {
            // ignore stats errors
        });
    }

    // ── Table Row Drag & Drop and Row Management Enhancement ──
    function initTableDragAndDrop() {
        if (!editorHolder) return;

        function attachRowHandles(tableEl) {
            var rows = tableEl.querySelectorAll('.tc-row');
            rows.forEach(function (row) {
                if (!row.querySelector('.tc-row-drag-handle')) {
                    var handle = document.createElement('div');
                    handle.className = 'tc-row-drag-handle';
                    handle.setAttribute('contenteditable', 'false');
                    handle.setAttribute('title', 'Satırı taşımak için sürükleyin');
                    handle.innerHTML = '<svg width="12" height="12" viewBox="0 0 24 24" fill="currentColor">' +
                        '<circle cx="8" cy="4" r="2"/>' +
                        '<circle cx="16" cy="4" r="2"/>' +
                        '<circle cx="8" cy="12" r="2"/>' +
                        '<circle cx="16" cy="12" r="2"/>' +
                        '<circle cx="8" cy="20" r="2"/>' +
                        '<circle cx="16" cy="20" r="2"/>' +
                        '</svg>';
                    // Append at end of row so row.firstChild remains a valid .tc-cell
                    row.appendChild(handle);
                }
            });
        }

        function enhanceRowPopover(popoverEl) {
            if (popoverEl.querySelector('.tc-popover__item--move-up')) return;

            var wrap = popoverEl.closest('.tc-wrap');
            if (!wrap) return;
            var tableEl = wrap.querySelector('.tc-table');
            if (!tableEl) return;

            // Find Delete row item to place Move buttons right before it
            var deleteItem = null;
            var items = popoverEl.querySelectorAll('.tc-popover__item');
            items.forEach(function (item) {
                var txt = item.textContent || '';
                if (txt.indexOf('Satır sil') !== -1 || txt.indexOf('Delete row') !== -1) {
                    deleteItem = item;
                }
            });

            // Move Up button
            var upItem = document.createElement('div');
            upItem.className = 'tc-popover__item tc-popover__item--move-up';
            upItem.innerHTML = '<span class="tc-popover__item-icon">' +
                '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">' +
                '<polyline points="18 15 12 9 6 15"></polyline>' +
                '</svg></span>' +
                '<span class="tc-popover__item-label">Yukarı taşı</span>';

            upItem.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                var selectedRow = tableEl.querySelector('.tc-row--selected');
                if (selectedRow && selectedRow.previousElementSibling && selectedRow.previousElementSibling.classList.contains('tc-row')) {
                    tableEl.insertBefore(selectedRow, selectedRow.previousElementSibling);
                    hasChanges = true;
                    updateStatus();
                    debouncedUpdateStats();
                }
                popoverEl.classList.remove('tc-popover--opened');
                if (selectedRow) selectedRow.classList.remove('tc-row--selected');
            });

            // Move Down button
            var downItem = document.createElement('div');
            downItem.className = 'tc-popover__item tc-popover__item--move-down';
            downItem.innerHTML = '<span class="tc-popover__item-icon">' +
                '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">' +
                '<polyline points="6 9 12 15 18 9"></polyline>' +
                '</svg></span>' +
                '<span class="tc-popover__item-label">Aşağı taşı</span>';

            downItem.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                var selectedRow = tableEl.querySelector('.tc-row--selected');
                if (selectedRow && selectedRow.nextElementSibling && selectedRow.nextElementSibling.classList.contains('tc-row')) {
                    tableEl.insertBefore(selectedRow.nextElementSibling, selectedRow);
                    hasChanges = true;
                    updateStatus();
                    debouncedUpdateStats();
                }
                popoverEl.classList.remove('tc-popover--opened');
                if (selectedRow) selectedRow.classList.remove('tc-row--selected');
            });

            if (deleteItem) {
                popoverEl.insertBefore(upItem, deleteItem);
                popoverEl.insertBefore(downItem, deleteItem);
            } else {
                popoverEl.appendChild(upItem);
                popoverEl.appendChild(downItem);
            }
        }

        function enhanceTables() {
            var tables = editorHolder.querySelectorAll('.tc-table');
            tables.forEach(function (tableEl) {
                attachRowHandles(tableEl);

                if (window.Sortable && !tableEl._sortableInstance) {
                    tableEl._sortableInstance = new Sortable(tableEl, {
                        animation: 150,
                        handle: '.tc-row-drag-handle',
                        draggable: '.tc-row',
                        ghostClass: 'tc-row-ghost',
                        chosenClass: 'tc-row-chosen',
                        dragClass: 'tc-row-dragging',
                        filter: '.tc-cell, .tc-toolbox, .tc-popover',
                        preventOnFilter: false,
                        swapThreshold: 0.65,
                        onEnd: function (evt) {
                            if (evt.oldIndex !== evt.newIndex) {
                                hasChanges = true;
                                updateStatus();
                                debouncedUpdateStats();
                            }
                        }
                    });
                }
            });

            // Check any open row popovers
            var popovers = editorHolder.querySelectorAll('.tc-toolbox--row .tc-popover');
            popovers.forEach(function (popover) {
                enhanceRowPopover(popover);
            });
        }

        // Initial scan
        enhanceTables();

        // Observe dynamic DOM changes (new tables, new rows, opened popovers)
        var observer = new MutationObserver(function (mutations) {
            var shouldEnhance = false;
            for (var i = 0; i < mutations.length; i++) {
                var m = mutations[i];
                if (m.type === 'attributes' && m.attributeName === 'class') {
                    if (m.target && m.target.classList && m.target.classList.contains('tc-popover--opened')) {
                        enhanceRowPopover(m.target);
                    }
                }
                if (m.addedNodes && m.addedNodes.length > 0) {
                    for (var j = 0; j < m.addedNodes.length; j++) {
                        var node = m.addedNodes[j];
                        if (node.nodeType === 1) {
                            if (node.classList.contains('tc-table') ||
                                node.classList.contains('tc-row') ||
                                node.classList.contains('tc-popover') ||
                                (node.querySelector && (node.querySelector('.tc-table') || node.querySelector('.tc-row') || node.querySelector('.tc-popover')))) {
                                shouldEnhance = true;
                                break;
                            }
                        }
                    }
                }
                if (shouldEnhance) break;
            }

            if (shouldEnhance) {
                enhanceTables();
            }
        });

        observer.observe(editorHolder, {
            childList: true,
            subtree: true,
            attributes: true,
            attributeFilter: ['class']
        });
    }

    // ── Unsaved changes warning ──
    window.addEventListener('beforeunload', function (e) {
        if (hasChanges) {
            e.preventDefault();
            e.returnValue = '';
        }
    });

})();

