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
            initTableRowReordering(editorHolder);
            initTableClipboardIntegration(editorHolder);
            console.log('Editor.js initialized with Table Row Reordering & Notion/Excel Paste');
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

    // ── Table Row Reordering inside Native Table Popover Menu ──
    function initTableRowReordering(container) {
        if (!container) return;

        function getSelectedRow(table, toolboxRow) {
            // 1. Check for .tc-row--selected
            var selected = table.querySelector('.tc-row--selected');
            if (selected) return selected;

            // 2. Fallback: match row position with row toolbox vertical position
            if (toolboxRow) {
                var toolboxTop = toolboxRow.offsetTop;
                var rows = Array.from(table.querySelectorAll('.tc-row'));
                var closestRow = null;
                var minDiff = 99999;
                rows.forEach(function (r) {
                    var diff = Math.abs(r.offsetTop - toolboxTop);
                    if (diff < minDiff) {
                        minDiff = diff;
                        closestRow = r;
                    }
                });
                if (minDiff < 50) return closestRow;
            }
            return null;
        }

        function updatePopoverItemsState(wrap) {
            var table = wrap.querySelector('.tc-table');
            var toolboxRow = wrap.querySelector('.tc-toolbox--row');
            var popover = toolboxRow ? toolboxRow.querySelector('.tc-popover') : null;
            if (!table || !popover) return;

            var row = getSelectedRow(table, toolboxRow);
            var itemUp = popover.querySelector('.tc-popover__item--move-up');
            var itemDown = popover.querySelector('.tc-popover__item--move-down');
            if (!itemUp || !itemDown) return;

            if (!row) {
                itemUp.classList.add('tc-popover__item--disabled');
                itemDown.classList.add('tc-popover__item--disabled');
                return;
            }

            var isWithHeadings = table.classList.contains('tc-table--with-headings') ||
                                 table.classList.contains('tc-table--heading') ||
                                 table.querySelector('.tc-row--heading') !== null;
            var rows = Array.from(table.querySelectorAll('.tc-row'));
            var idx = rows.indexOf(row);
            var startIndex = isWithHeadings ? 1 : 0;

            if (idx <= startIndex) {
                itemUp.classList.add('tc-popover__item--disabled');
            } else {
                itemUp.classList.remove('tc-popover__item--disabled');
            }

            if (idx === -1 || idx >= rows.length - 1 || (isWithHeadings && idx === 0)) {
                itemDown.classList.add('tc-popover__item--disabled');
            } else {
                itemDown.classList.remove('tc-popover__item--disabled');
            }
        }

        function injectMenuItemsIntoPopover(popover, wrap) {
            if (!popover || popover.querySelector('.tc-popover__item--move-up')) return;

            // Create Move Up item
            var itemUp = document.createElement('div');
            itemUp.className = 'tc-popover__item tc-popover__item--move-up';
            itemUp.innerHTML = '<span class="tc-popover__item-icon">' +
                '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round"><line x1="12" y1="19" x2="12" y2="5"></line><polyline points="5 12 12 5 19 12"></polyline></svg>' +
                '</span><span class="tc-popover__item-label">Satırı yukarı taşı</span>';

            // Create Move Down item
            var itemDown = document.createElement('div');
            itemDown.className = 'tc-popover__item tc-popover__item--move-down';
            itemDown.innerHTML = '<span class="tc-popover__item-icon">' +
                '<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.2" stroke-linecap="round" stroke-linejoin="round"><line x1="12" y1="5" x2="12" y2="19"></line><polyline points="19 12 12 19 5 12"></polyline></svg>' +
                '</span><span class="tc-popover__item-label">Satırı aşağı taşı</span>';

            itemUp.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                if (itemUp.classList.contains('tc-popover__item--disabled')) return;

                var table = wrap.querySelector('.tc-table');
                var toolboxRow = wrap.querySelector('.tc-toolbox--row');
                var row = getSelectedRow(table, toolboxRow);
                if (!row || !table) return;

                var prevRow = row.previousElementSibling;
                var isWithHeadings = table.classList.contains('tc-table--with-headings') ||
                                     table.classList.contains('tc-table--heading') ||
                                     table.querySelector('.tc-row--heading') !== null;

                if (prevRow && prevRow.classList.contains('tc-row')) {
                    if (isWithHeadings && prevRow === table.querySelector('.tc-row:first-child')) {
                        return;
                    }
                    table.insertBefore(row, prevRow);
                    highlightRow(row);
                    onTableChanged();
                    closePopover(popover);
                }
            });

            itemDown.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();
                if (itemDown.classList.contains('tc-popover__item--disabled')) return;

                var table = wrap.querySelector('.tc-table');
                var toolboxRow = wrap.querySelector('.tc-toolbox--row');
                var row = getSelectedRow(table, toolboxRow);
                if (!row || !table) return;

                var nextRow = row.nextElementSibling;
                if (nextRow && nextRow.classList.contains('tc-row')) {
                    table.insertBefore(nextRow, row);
                    highlightRow(row);
                    onTableChanged();
                    closePopover(popover);
                }
            });

            // Prepend before existing items ("Üste satır ekle" etc.)
            popover.insertBefore(itemDown, popover.firstChild);
            popover.insertBefore(itemUp, popover.firstChild);
        }

        function closePopover(popover) {
            popover.classList.remove('tc-popover--opened');
        }

        function highlightRow(row) {
            row.classList.add('tc-row--moved');
            setTimeout(function () {
                row.classList.remove('tc-row--moved');
            }, 500);
        }

        function onTableChanged() {
            hasChanges = true;
            updateStatus();
            debouncedUpdateStats();
        }

        function scanAndEnhance() {
            var wraps = container.querySelectorAll('.tc-wrap');
            wraps.forEach(function (wrap) {
                var toolboxRow = wrap.querySelector('.tc-toolbox--row');
                if (toolboxRow) {
                    var popover = toolboxRow.querySelector('.tc-popover');
                    if (popover) {
                        injectMenuItemsIntoPopover(popover, wrap);
                    }
                    var toggler = toolboxRow.querySelector('.tc-toolbox__toggler');
                    if (toggler && !toggler.dataset.hasMoveListener) {
                        toggler.dataset.hasMoveListener = 'true';
                        toggler.addEventListener('mouseenter', function () {
                            updatePopoverItemsState(wrap);
                        });
                        toggler.addEventListener('click', function () {
                            setTimeout(function () {
                                updatePopoverItemsState(wrap);
                            }, 10);
                        });
                    }
                }
            });
        }

        // Observe dynamic changes
        var observer = new MutationObserver(function () {
            scanAndEnhance();
        });

        observer.observe(container, { childList: true, subtree: true });

        // Initial scans
        setTimeout(scanAndEnhance, 200);
        setTimeout(scanAndEnhance, 600);
        setTimeout(scanAndEnhance, 1200);
    }

    // ── Semantic Table HTML Converter (Converts Editor.js Grid Divs to standard HTML table) ──
    function convertEditorTablesToSemanticHtml(element) {
        var clone = element.cloneNode(true);

        // Remove Editor.js internal UI elements from the copied fragment
        var uiElements = clone.querySelectorAll('.tc-toolbox, .tc-row-reorder-actions, .tc-add-row, .tc-add-column, .ce-toolbar, .ce-block__tune-container, .ce-popover, .ce-block__tune, .ce-settings');
        uiElements.forEach(function (el) { el.remove(); });

        // Convert all .tc-table (which use CSS Grid divs) into standard HTML <table>
        var tables = clone.querySelectorAll('.tc-table');
        tables.forEach(function (table) {
            var isHeading = table.classList.contains('tc-table--with-headings') ||
                            table.classList.contains('tc-table--heading') ||
                            table.querySelector('.tc-row--heading') !== null;

            var htmlTable = document.createElement('table');
            htmlTable.setAttribute('border', '1');
            htmlTable.style.borderCollapse = 'collapse';
            htmlTable.style.width = '100%';

            var thead = isHeading ? document.createElement('thead') : null;
            var tbody = document.createElement('tbody');

            var rows = Array.from(table.querySelectorAll('.tc-row'));
            rows.forEach(function (row, rowIdx) {
                var tr = document.createElement('tr');
                var cells = Array.from(row.querySelectorAll('.tc-cell'));
                var isHeaderRow = (isHeading && rowIdx === 0);
                var cellTag = isHeaderRow ? 'th' : 'td';

                cells.forEach(function (cell) {
                    var c = document.createElement(cellTag);
                    c.innerHTML = cell.innerHTML.trim();
                    tr.appendChild(c);
                });

                if (isHeaderRow && thead) {
                    thead.appendChild(tr);
                } else {
                    tbody.appendChild(tr);
                }
            });

            if (thead) htmlTable.appendChild(thead);
            htmlTable.appendChild(tbody);

            var wrap = table.closest('.tc-wrap') || table;
            if (wrap && wrap.parentNode) {
                wrap.parentNode.replaceChild(htmlTable, wrap);
            }
        });

        return clone;
    }

    // ── Table Copy & Paste Integration (Notion / Excel / HTML Table support) ──
    function initTableClipboardIntegration(container) {
        if (!container) return;

        function cleanCellContent(html) {
            if (!html) return '';
            var div = document.createElement('div');
            div.innerHTML = html;
            // Remove scripts/styles/meta but preserve formatted text
            var badElements = div.querySelectorAll('script, style, meta, link, noscript, svg');
            badElements.forEach(function (el) { el.remove(); });
            return div.innerHTML.trim();
        }

        function parseTableFromClipboard(clipboardData) {
            if (!clipboardData) return null;

            var html = '';
            var plain = '';
            try { html = clipboardData.getData('text/html') || ''; } catch (e) {}
            try { plain = clipboardData.getData('text/plain') || ''; } catch (e) {}

            // 1. Try HTML (Notion, Google Docs, Excel, Word, Web)
            if (html && html.trim()) {
                try {
                    var parser = new DOMParser();
                    var doc = parser.parseFromString(html, 'text/html');

                    // A. Standard <table>
                    var table = doc.querySelector('table');
                    if (table) {
                        var rows = Array.from(table.querySelectorAll('tr'));
                        if (rows.length > 0) {
                            var hasHeadings = table.querySelector('th') !== null || (table.querySelector('thead') !== null);
                            var content = [];
                            var maxCols = 0;

                            rows.forEach(function (tr) {
                                var cells = Array.from(tr.querySelectorAll('td, th'));
                                if (cells.length > 0) {
                                    var rowData = cells.map(function (c) {
                                        return cleanCellContent(c.innerHTML) || c.textContent.trim();
                                    });
                                    if (rowData.length > maxCols) maxCols = rowData.length;
                                    content.push(rowData);
                                }
                            });

                            if (content.length > 0 && maxCols > 0) {
                                content.forEach(function (row) {
                                    while (row.length < maxCols) row.push('');
                                });
                                return { withHeadings: hasHeadings, content: content };
                            }
                        }
                    }

                    // B. Notion role="table" / role="row" or [class*="notion-table"]
                    var roleRows = Array.from(doc.querySelectorAll('[role="row"], .notion-table-row, [class*="notion-table-row"]'));
                    if (roleRows.length > 0) {
                        var content = [];
                        var maxCols = 0;
                        var hasHeadings = doc.querySelector('[role="columnheader"]') !== null;

                        roleRows.forEach(function (rRow) {
                            var cells = Array.from(rRow.querySelectorAll('[role="cell"], [role="columnheader"], [role="gridcell"], .notion-table-cell, [class*="notion-table-cell"]'));
                            if (cells.length > 0) {
                                var rowData = cells.map(function (c) {
                                    return cleanCellContent(c.innerHTML) || c.textContent.trim();
                                });
                                if (rowData.length > maxCols) maxCols = rowData.length;
                                content.push(rowData);
                            }
                        });

                        if (content.length > 0 && maxCols > 0) {
                            content.forEach(function (row) {
                                while (row.length < maxCols) row.push('');
                            });
                            return { withHeadings: hasHeadings, content: content };
                        }
                    }
                } catch (e) {
                    console.warn('HTML Table parsing error:', e);
                }
            }

            // 2. Try Plain Text (TSV from Notion/Excel or Markdown Table)
            if (plain && plain.trim()) {
                var rawLines = plain.split(/\r?\n/).map(function (l) { return l.trim(); }).filter(Boolean);

                // A. Check for Markdown Table (| col 1 | col 2 |)
                var mdLines = rawLines.filter(function (l) { return l.startsWith('|') && l.endsWith('|'); });
                if (mdLines.length >= 2) {
                    var content = [];
                    var maxCols = 0;
                    var hasHeadings = false;

                    mdLines.forEach(function (line) {
                        if (/^\|[\s\-:|]+\|$/.test(line)) {
                            hasHeadings = true;
                            return;
                        }
                        var parts = line.slice(1, -1).split('|').map(function (p) { return p.trim(); });
                        if (parts.length > maxCols) maxCols = parts.length;
                        content.push(parts);
                    });

                    if (content.length > 0 && maxCols > 1) {
                        content.forEach(function (row) {
                            while (row.length < maxCols) row.push('');
                        });
                        return { withHeadings: hasHeadings, content: content };
                    }
                }

                // B. Check for Tab-Separated Values (TSV from Notion / Excel)
                if (plain.indexOf('\t') !== -1) {
                    var lines = plain.split(/\r?\n/).filter(function (line) {
                        return line.length > 0;
                    });

                    if (lines.length > 0) {
                        var content = [];
                        var maxCols = 0;

                        lines.forEach(function (line) {
                            var cols = line.split('\t').map(function (c) {
                                return c.trim();
                            });
                            if (cols.length > maxCols) maxCols = cols.length;
                            content.push(cols);
                        });

                        if (maxCols > 1 && content.length > 0) {
                            content.forEach(function (row) {
                                while (row.length < maxCols) row.push('');
                            });
                            return {
                                withHeadings: false,
                                content: content
                            };
                        }
                    }
                }
            }

            return null;
        }

        function pasteIntoExistingTable(startCell, tableData) {
            var table = startCell.closest('.tc-table');
            if (!table) return;

            var startRow = startCell.closest('.tc-row');
            var rows = Array.from(table.querySelectorAll('.tc-row'));
            var startRowIdx = rows.indexOf(startRow);
            var startCells = Array.from(startRow.querySelectorAll('.tc-cell'));
            var startColIdx = startCells.indexOf(startCell);

            if (startRowIdx === -1 || startColIdx === -1) return;

            tableData.content.forEach(function (pRow, rOffset) {
                var targetRowIdx = startRowIdx + rOffset;
                var rowEl = rows[targetRowIdx];

                if (rowEl) {
                    var cells = Array.from(rowEl.querySelectorAll('.tc-cell'));
                    pRow.forEach(function (val, cOffset) {
                        var targetColIdx = startColIdx + cOffset;
                        if (cells[targetColIdx]) {
                            cells[targetColIdx].innerHTML = val;
                        }
                    });
                }
            });

            table.dispatchEvent(new Event('input', { bubbles: true }));
            hasChanges = true;
            updateStatus();
            debouncedUpdateStats();
        }

        // Intercept paste events at the document level in the capture phase
        document.addEventListener('paste', function (e) {
            // Only process if paste target is within the editor
            if (!container.contains(e.target) && e.target !== container) {
                return;
            }

            var clipboardData = e.clipboardData || window.clipboardData;
            if (!clipboardData) return;

            var tableData = parseTableFromClipboard(clipboardData);
            if (!tableData || !tableData.content || tableData.content.length === 0) {
                return; // Not a table, let Editor.js / Image tool handle normal paste
            }

            // CRITICAL: Stop immediate propagation so ImageTool or file uploader is never invoked
            e.preventDefault();
            e.stopPropagation();
            if (e.stopImmediatePropagation) {
                e.stopImmediatePropagation();
            }

            var targetCell = e.target.closest ? e.target.closest('.tc-cell') : null;
            if (targetCell) {
                pasteIntoExistingTable(targetCell, tableData);
                return;
            }

            if (!editor || !editor.blocks) return;

            var currentIdx = -1;
            try {
                currentIdx = editor.blocks.getCurrentBlockIndex();
            } catch (err) {
                currentIdx = -1;
            }

            if (currentIdx < 0) {
                try {
                    currentIdx = editor.blocks.getBlocksCount();
                } catch (err) {
                    currentIdx = 0;
                }
            }

            var currentBlock = null;
            try {
                currentBlock = editor.blocks.getBlockByIndex(currentIdx);
            } catch (err) {
                currentBlock = null;
            }

            var isCurrentEmpty = false;
            if (currentBlock && currentBlock.holder) {
                var text = currentBlock.holder.textContent.trim();
                if (!text) isCurrentEmpty = true;
            }

            if (isCurrentEmpty) {
                editor.blocks.delete(currentIdx);
                editor.blocks.insert('table', tableData, {}, currentIdx, true);
            } else {
                editor.blocks.insert('table', tableData, {}, currentIdx + 1, true);
            }

            hasChanges = true;
            updateStatus();
            debouncedUpdateStats();
        }, true);

        // Intercept copy events to format Editor.js tables as standard HTML tables (for Notion, Excel, Word)
        document.addEventListener('copy', function (e) {
            var selection = window.getSelection();
            if (!selection || selection.rangeCount === 0 || selection.isCollapsed) return;

            var range = selection.getRangeAt(0);
            var intersectsEditor = container.contains(range.commonAncestorContainer) ||
                                   range.commonAncestorContainer === container ||
                                   container.contains(range.startContainer) ||
                                   container.contains(range.endContainer);

            if (!intersectsEditor) return;

            var fragment = range.cloneContents();
            var hasTable = fragment.querySelector('.tc-table, .tc-wrap') !== null;

            if (!hasTable) {
                var tableParent = range.commonAncestorContainer.nodeType === 1
                    ? range.commonAncestorContainer.closest('.tc-table')
                    : (range.commonAncestorContainer.parentElement ? range.commonAncestorContainer.parentElement.closest('.tc-table') : null);
                if (!tableParent) return;
            }

            var tempDiv = document.createElement('div');
            tempDiv.appendChild(fragment);

            var convertedDiv = convertEditorTablesToSemanticHtml(tempDiv);
            var fullHtml = convertedDiv.innerHTML;
            var plainText = convertedDiv.innerText || convertedDiv.textContent;

            if (e.clipboardData) {
                e.clipboardData.setData('text/html', fullHtml);
                e.clipboardData.setData('text/plain', plainText);
                e.preventDefault();
            }
        });
    }

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

    // ── Copy Entire Article Button ──
    var btnCopyArticle = document.getElementById('btnCopyArticle');
    if (btnCopyArticle) {
        btnCopyArticle.addEventListener('click', async function (e) {
            e.preventDefault();
            if (!editor) return;

            try {
                var title = titleInput ? titleInput.value.trim() : '';
                
                // Clone editor DOM and convert tables to semantic HTML
                var convertedDiv = convertEditorTablesToSemanticHtml(editorHolder);
                
                // Build semantic HTML string
                var htmlParts = [];
                if (title) {
                    htmlParts.push('<h1>' + title + '</h1>');
                }
                htmlParts.push(convertedDiv.innerHTML);
                var fullHtml = htmlParts.join('\n');

                // Build plain text / Markdown string
                var textParts = [];
                if (title) {
                    textParts.push('# ' + title);
                    textParts.push('');
                }
                textParts.push(convertedDiv.innerText || convertedDiv.textContent);
                var fullPlain = textParts.join('\n');

                // Write to clipboard
                if (navigator.clipboard && window.ClipboardItem) {
                    var item = new ClipboardItem({
                        'text/html': new Blob([fullHtml], { type: 'text/html' }),
                        'text/plain': new Blob([fullPlain], { type: 'text/plain' })
                    });
                    await navigator.clipboard.write([item]);
                } else if (navigator.clipboard && navigator.clipboard.writeText) {
                    await navigator.clipboard.writeText(fullPlain);
                }

                // Show success feedback
                var originalHtml = btnCopyArticle.innerHTML;
                btnCopyArticle.classList.add('copied');
                btnCopyArticle.innerHTML = '<i class="bi bi-check2"></i> <span>Kopyalandı!</span>';

                setTimeout(function () {
                    btnCopyArticle.classList.remove('copied');
                    btnCopyArticle.innerHTML = originalHtml;
                }, 2000);
            } catch (err) {
                console.error('Makale kopyalama hatası:', err);
                btnCopyArticle.innerHTML = '<i class="bi bi-x-circle"></i> <span>Hata!</span>';
                setTimeout(function () {
                    btnCopyArticle.innerHTML = '<i class="bi bi-clipboard"></i> <span>Kopyala</span>';
                }, 2000);
            }
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


