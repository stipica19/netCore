
$(function () {
    
    $(document).on('click', '.delete', function ()
    { if (!confirm("Obrisati zapis?")) { event.preventDefault(); } });
});
