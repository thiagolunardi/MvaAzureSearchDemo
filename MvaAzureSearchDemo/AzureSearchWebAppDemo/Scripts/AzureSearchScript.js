function Search(categoryFilter) {

    $('#resultado tbody').html('');
    $('#facets').hide();

    var textSearch = $('#textSearch').val();

    $.get(window.apiUrl + '/search/search?searchText=' + textSearch + '&category=' + categoryFilter,
        function(response) {
            
            $('#TotalCount').html(response.Count);

            var posts = '';

            for (var idx = 0; idx < response.Data.length; idx++) {
                var post = response.Data[idx];
                posts = posts +
                    '<tr>' +
                    '<td><a href="' + post.Guid + '">' + post.Title + '</a><br/><small>' + post.Description + '</small></td>' +
                    '<td class="no-wrap">' + post.Category + '</td>' +
                    '<td>' + formatedDate(post.Date) + '</td>' +
                    '<td>' + Math.round(post.Score*100)/100 + '</td>' +
                    '</tr>';
            }

            $('#resultado tbody').html(posts);

            var $categorias = $('#categorias');
            $categorias.html('');
            
            if (response.Facets == null) return;
            
            $('#facets').show();

            for (var idx = 0; idx < response.Facets.length; idx++) {
                var category = response.Facets[idx];

                var $item = $('<a href="#" onclick="Search(\''+ category.Value +'\');" class="list-group-item"><span class="badge">' + category.Count + '</span>' + category.Value + '</a>');
                $item.appendTo($categorias);
            }
        });
}

$(function () {

    $('form[role=form]').submit(function(evt) {
        evt.preventDefault();
        return false;
    });

    $("#textSearch").keyup(function (event) {
        if (event.keyCode === 13) {
            Search('');
        }
    });
    
    $('#btnBuscar').click(function () {
        Search('');
    });

    var suggester = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.whitespace,
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: window.apiUrl + '/search/suggest?searchText=%QUERY',
            wildcard: '%QUERY'
        }
    });

    $('#suggestForm .typeahead').typeahead(null, {
        name: 'posts',
        display: 'title',
        source: suggester
});

    $('#suggestForm .typeahead').bind('typeahead:select', function (ev, suggestion) {
        window.location.href = suggestion.guid;
    });
});

function formatedDate(date) {
    date = new Date(date);
    return date.getDate() + '/' + date.getMonth() + '/' + date.getFullYear();
}