document.addEventListener('DOMContentLoaded', function () {
    const toggles = document.querySelectorAll('.showcase-toggle');
    
    toggles.forEach(toggle => {
        toggle.addEventListener('change', function (e) {
            e.preventDefault();
            
            const id = this.getAttribute('data-id');
            const isChecked = this.checked;
            const switchElem = this;
            
            Swal.fire({
                title: 'Emin misiniz?',
                text: isChecked ? 'Bu ürün ana sayfada (vitrinde) gösterilecek.' : 'Bu ürün vitrinden (ana sayfadan) kaldırılacak.',
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Evet, Onaylıyorum',
                cancelButtonText: 'İptal'
            }).then((result) => {
                if (result.isConfirmed) {
                    // AJAX Request
                    fetch('/Admin/ProductManagement/ToggleShowcase', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                        },
                        body: new URLSearchParams({
                            'id': id,
                            'state': isChecked
                        })
                    })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            Swal.fire('Başarılı!', data.message, 'success');
                        } else {
                            switchElem.checked = !isChecked; // Revert
                            Swal.fire('Hata!', data.message || 'Bir hata oluştu.', 'error');
                        }
                    })
                    .catch(error => {
                        switchElem.checked = !isChecked; // Revert
                        Swal.fire('Hata!', 'Sunucu ile bağlantı kurulamadı.', 'error');
                    });
                } else {
                    // Cancelled, revert toggle
                    switchElem.checked = !isChecked;
                }
            });
        });
    });
});
