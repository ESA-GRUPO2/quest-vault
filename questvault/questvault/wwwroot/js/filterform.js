function clearForm() {
    document.getElementById("filterForm").reset();
}

// selectpicker for select in forms

$('.select-results').selectpicker({
    dropupAuto: false,
    liveSearch: true,
    size: 7,
    style: 'btn-dark',
});
