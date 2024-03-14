$(document).ready(function () {
    $('#searchTerm').on('input', function () {
        var searchTerm = $(this).val();
        console.log('Valor da pesquisa:', searchTerm);

        $.ajax({
            type: 'POST',
            url: '/TesteIGDB/Search',
            data: { searchTerm: searchTerm },
            success: function (data) {
                // Atualize a drop-down com os resultados recebidos
                // Exemplo b�sico: assumindo que 'data.Jogos' � a lista de jogos
                $('#searchResults').empty();
                $.each(data.Jogos, function (index, jogo) {
                    $('#searchResults').append('<div>' + jogo.Nome + '</div>');
                });
            },
            error: function (xhr, textStatus, errorThrown) {
                console.error(xhr.responseText);
                console.error(textStatus);
                console.error(errorThrown);
            }
        });
    });
});

