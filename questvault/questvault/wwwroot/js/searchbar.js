$(document).ready(function () {
    $('#searchInput').on('input', function () {
        var searchTerm = $(this).val();
        console.log('Valor da pesquisa:', searchTerm);

        if (searchTerm.length >= 2) {
            $.ajax({
                url: 'Games/search',
                type: 'GET',
                data: { searchTerm: searchTerm },
                success: function (data) {
                    $('#searchResults').empty();
                    $.each(data, function (index, game) {
                        $('#searchResults').append('<option value="' + game.id + '">' + game.name + '</option>');
                    })
                }
            });
        }
    });
});

