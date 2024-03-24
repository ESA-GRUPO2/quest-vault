$(document).ready(function () {
    $('#searchInput').on('input', function () {
        var searchTerm = $(this).val();
        console.log('Valor da pesquisa:', searchTerm);

        if (searchTerm.length >= 2) {
            $.ajax({
                url: '/Games/search',
                type: 'GET',
                data: { searchTerm: searchTerm },
                success: function (data) {
                    $('#searchResults').empty();
                    if (data.length > 0) {
                        $.each(data, function (index, game) {
                            $('#searchResults').append('<li class="list-group-item"><a href="/Games/Details/' + game.igdbId + '">' + game.name + '</a></li>');
                        });
                        $('#searchResults').show(); // Mostrar a dropdown se houver resultados
                    } else {
                        $('#searchResults').hide(); // Esconder a dropdown se não houver resultados
                    }
                }
            });
        } else {
            $('#searchResults').empty().hide(); // Limpar e esconder a dropdown se o campo de pesquisa estiver vazio
        }

        // Manipulador de eventos para o evento 'blur' na barra de pesquisa
        $('#searchInput').on('blur', function () {
            $('#searchResults').empty().hide(); // Limpar e esconder a dropdown quando a barra de pesquisa perde o foco
        });
    });

    $('#searchInput').on('keydown', function (e) {
        if ($('#searchResults').is(':visible')) {
            var results = $('#searchResults li');
            var selectedIndex = results.filter('.selected').index();

            switch (e.key) {
                case 'ArrowUp': // Seta para cima
                    selectedIndex = (selectedIndex === -1) ? (results.length - 1) : (selectedIndex - 1);
                    break;
                case 'ArrowDown': // Seta para baixo
                    selectedIndex = (selectedIndex === results.length - 1) ? -1 : (selectedIndex + 1);
                    break;
                case 'Enter': // Enter
                    if (selectedIndex !== -1) {
                        var selectedLink = results.eq(selectedIndex).find('a');
                        if (selectedLink.length > 0) {
                            window.location.href = selectedLink.attr('href');
                        }
                    }
                    break;
            }

            results.removeClass('selected');
            if (selectedIndex !== -1) {
                results.eq(selectedIndex).addClass('selected');
            }
        }
    });

});


//$(document).ready(function () {
//    $('#searchInput').on('input', function () {
//        var searchTerm = $(this).val();
//        console.log('Valor da pesquisa:', searchTerm);

//        if (searchTerm.length >= 2) {
//            $.ajax({
//                url: '/Games/search',
//                type: 'GET',
//                data: { searchTerm: searchTerm },
//                success: function (data) {
//                    $('#searchResults').empty();
//                    $.each(data, function (index, game) {
//                        $('#searchResults').append(
//                            '<li class="list-group-item"><a href="/Games/Details/' + game.gameId + '">' + game.name + '</a></li>'
//                        );
//                    })
//                }
//            });
//        }
//    });
//});

//$(document).ready(function () {
//    $('#searchInput').on('input', function () {
//        var searchTerm = $(this).val();
//        console.log('Valor da pesquisa:', searchTerm);

//        if (searchTerm.length >= 2) {
//            $.ajax({
//                url: '/Games/search',
//                type: 'GET',
//                data: { searchTerm: searchTerm },
//                success: function (data) {
//                    $('#searchResults').empty();
//                    if (data.length > 0) {
//                        $.each(data, function (index, game) {
//                            $('#searchResults').append('<li class="list-group-item"><a href="/Games/Details/' + game.id + '">' + game.name + '</a></li>');
//                        });
//                        $('#searchResults').show(); // Mostrar a dropdown se houver resultados
//                    } else {
//                        $('#searchResults').hide(); // Esconder a dropdown se não houver resultados
//                    }
//                }
//            });
//        } else {
//            $('#searchResults').empty().hide(); // Limpar e esconder a dropdown se o campo de pesquisa estiver vazio
//        }
//    });
//});
