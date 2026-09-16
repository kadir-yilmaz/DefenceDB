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

    // ── Unsaved changes warning ──
    window.addEventListener('beforeunload', function (e) {
        if (hasChanges) {
            e.preventDefault();
            e.returnValue = '';
        }
    });

})();
