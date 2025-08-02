let pageNumber = 1;
let pageSize = 10;
let searchBy = '';
let categoryId = 0;

function getMyVideos() {
    const parameters = {
        pageNumber,
        pageSize,
        searchBy,
        categoryId
    };

    $.ajax({
        url: "/Home/GetVideosForHomeGrid",
        type: "GET",
        data: parameters,
        success: function (data) {
            const result = data.result;
            console.log(data);

            $('#videosTableBody').empty();
            $('#paginationSummary').empty();
            $('#paginationBtnGroup').empty();
            $('#itemsPerPageDisplay').empty();

            populateVideoTableBody(result.items);

            if (result.totalItemsCount > 0) {
                $('#itemsPerPageDisplay').text(pageSize);

                const from = (result.pageNumber - 1) * result.pageSize + 1;
                const to = result.pageNumber * result.pageSize > result.totalItemsCount ? result.totalItemsCount : result.pageNumber * result.pageSize;

                $('#paginationSummary').text(`${from}-${to} of ${result.totalItemsCount}`);

                // First Page Button
                let firstPageBtn = `
                            <button type="button" class="btn btn-secondary btn-sm paginationBtn" ${result.pageNumber == 1 ? "disabled" : ''}
                                data-value="1" data-bs-toggle="tooltip" data-bs-placement="top" title="First Page">
                                <i class="bi bi-chevron-bar-left"></i>
                            </button>
                        `;
                $('#paginationBtnGroup').append(firstPageBtn);

                // Previous Page Button
                let previousPageBtn = `
                            <button class="btn btn-sm btn-secondary paginationBtn" ${result.pageNumber == 1 ? "disabled" : ''}
                                data-value="${result.pageNumber - 1}" data-bs-toggle="tooltip" data-bs-placement="top" title="Previous Page">
                                <i class="bi bi-chevron-left"></i>
                            </button>
                        `;
                $('#paginationBtnGroup').append(previousPageBtn);

                // Next Page Button
                let nextPageBtn = `
                            <button class="btn btn-sm btn-secondary paginationBtn" ${result.pageNumber == result.totalPages ? "disabled" : ''}
                                data-value="${result.pageNumber + 1}" data-bs-toggle="tooltip" data-bs-placement="top" title="Next Page">
                                <i class="bi bi-chevron-right"></i>
                            </button>
                        `;
                $('#paginationBtnGroup').append(nextPageBtn);

                // Last Page Button
                let lastPageBtn = `
                            <button type="button" class="btn btn-sm btn-secondary paginationBtn" ${result.pageNumber == result.totalPages ? 'disabled' : ''}
                                data-value="${result.totalPages}" data-bs-toggle="tooltip" data-bs-placement="top" title="Last Page">
                                <i class="bi bi-chevron-bar-right"></i>
                            </button>
                        `;
                $('#paginationBtnGroup').append(lastPageBtn);

                // On paginationBtn click event
                $('.paginationBtn').click(function () {
                    pageNumber = $(this).data('value');
                    getMyVideos();
                });
            }
            else {
                $('#itemsPerPageDropdown').hide();
            }
        }
    });

    // On dropdown "Rows per page" selection event
    $('.pageSizeBtn').click(function () {
        pageSize = $(this).data('value');
        getMyVideos();
    });

    // On dropdown "CategoryDropdown" selection event
    $('#categoryDropdown').on('change', function () {
        var selectedValue = $(this).val(); // Get selected value
        categoryId = selectedValue;

        getMyVideos();
    });

    // On searchBtn click
    $('#searchBtn').click(function () {
        const searchInput = $('#searchInput').val();
        searchBy = searchInput;
        getMyVideos();
    })

    // On search input when enter is pressed
    $('#searchInput').on('keyup', function (event) {
        if (event.key === 'Enter' || event.keyCode === 13) {
            var searchInput = $(this).val();
            searchBy = searchInput;
            getMyVideos();
        }
    })

    function populateVideoTableBody(videos) {
        let divTag = "";
        if (videos.length > 0) {
            videos.forEach((v, index) => {
                if (index % 4 === 0) {
                    divTag += `<div class="row">`;
                }
                divTag += `
                    <div class="col-xl-3 col-md-6 pt-2">
                        <div class="p-2 border rounded text-center">
                            <div>
                                <a href="/Video/Watch/${v.id}">
                                    <img src="${v.thumbnailUrl}" class="rounded preview-image" alt="video thumbnail" />
                                </a>
                            </div>
                            <div class="text-danger-emphasis" style="text-decoration: none;">
                                ${v.title}
                            </div>
                            <div>
                                <span style="font-size: small">
                                    <a href="/Member/Channel/${v.channelId}" style="text-decoration: none;" class="text-primary">${v.channelName}</a><br />
                                    ${formatView(v.views)} Views - ${timeAgo(v.createdAt)}
                                </span>
                            </div>
                        </div>
                    </div>
                `;
                if ((index + 1) % 4 === 0 || index === videos.length - 1) {
                    divTag += `</div>`;
                }
            });
            $('#videosTableBody').append(divTag);
        } else {
            $('#videosTableBody').append(`
                <div class="row">
                    <div class="col text-center">No Videos</div>
                </div>
            `);
        }
    }
}