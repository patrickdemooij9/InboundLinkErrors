app.requires.push('ngTable');

angular.module("umbraco").controller("LinkErrorsController", function ($scope, $filter, LinkErrorsApi, ngTableParams, editorService) {

    //Property to display error messages
    $scope.errorMessage = '';
    //App state
    $scope.initialLoad = false;
    $scope.cacheCleared = false;

    $scope.showHidden = false;
    $scope.showMedia = false;

    $scope.refreshTable = function () {
        if (!$scope.tableParams) return;

        $scope.tableParams.reload();
    }

    $scope.clearCache = function () {
        $scope.cacheCleared = true;
        return LinkErrorsApi.clearCache().then($scope.fetchLinkErrors.bind(this));
    }

    $scope.fetchLinkErrors = function () {
        return LinkErrorsApi.getAll().then($scope.onRecieveAllLinkErrorsResponse.bind(this));
    };

    $scope.onRecieveAllLinkErrorsResponse = function (response) {
        if (!response || !response.data) {
            $scope.errorMessage = "Error fetching link errors from server";
            return;
        }

        $scope.linkErrors = response.data;
        $scope.refreshTable();
    }

    $scope.openRedirectDialog = function (linkError) {
        var redirectDialogOptions = {
            title: "Set redirect",
            view: "/App_Plugins/LinkErrors/assets/views/createRedirect.html",
            size: "small",
            submit: function (model) {
                var selectedNodeId = model.selectedNode.id;
                var selectedCulture = model.selectedCulture.culture;
                editorService.close();

                LinkErrorsApi.setRedirect(linkError.Id, selectedNodeId, selectedCulture).then($scope.onSetRedirectResponse.bind(this, linkError));
            },
            close: function () {
                editorService.close();
            }
        };
        editorService.open(redirectDialogOptions);
    }

    $scope.onSetRedirectResponse = function (linkError, response) {
        if (!response || !response.data || !response.data.Success) {
            $scope.errorMessage = "Error sending request to create redirect";
            return;
        }

        $scope.removeLinkError(linkError);
    }

    $scope.deleteLinkError = function (linkError) {
        if (confirm("Are you sure you want to delete this?")) {
            LinkErrorsApi.remove(linkError.Id).then($scope.onDeleteLinkResponse.bind(this, linkError));
        }
    }

    $scope.onDeleteLinkResponse = function (linkError, response) {
        if (!response || !response.data) {
            $scope.errorMessage = "Error sending request to delete.";
            return;
        }

        if (response.data.Success) {
            $scope.errorMessage = '';
            $scope.removeLinkError(linkError);
        }
        else {
            $scope.errorMessage = response.data.ErrorMessage;
        }
    }

    $scope.toggleHideLinkError = function (linkError, toggle) {
        LinkErrorsApi.hide(linkError.Id, toggle).then($scope.onHideLinkResponse.bind(this, linkError, toggle));
    }

    $scope.onHideLinkResponse = function (linkError, toggle, response) {
        if (!response || !response.data) {
            $scope.errorMessage = "Error sending request to hide.";
            return;
        }

        if (response.data.Success) {
            $scope.errorMessage = '';
            linkError.IsHidden = toggle;
            $scope.refreshTable();
        } else {
            $scope.errorMessage = response.data.ErrorMessage;
        }
    }

    $scope.removeLinkError = function (linkError) {
        var index = $scope.linkErrors.indexOf(linkError);
        if (index > -1) {
            $scope.linkErrors.splice(index, 1);
            $scope.tableParams.total($scope.linkErrors.length);
            $scope.tableParams.reload();
        }
    }

    /*
    * Defines a new ngTable. 
    */
    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10,          // count per page
        sorting: {
            LastUpdated: 'desc'     // initial sorting
        },
        filter: {
            Message: ''       // initial filter
        },
        data: $scope.initialData
    }, {
            total: 0,
            getData: function ($defer, params) {
                var data = $scope.linkErrors || [];

                var searchTerm = params.filter().Search;
                var searchedData = searchTerm ?
                    data.filter(function (linkError) {
                        return linkError.Url.toLowerCase().indexOf(searchTerm.toLowerCase()) > -1;
                    }) : data;

                var filteredData = $scope.showHidden ? searchedData :
                    searchedData.filter(function (linkError) {
                        return !linkError.IsHidden;
                    });

                filteredData = $scope.showMedia
                    ? filteredData
                    : filteredData.filter(function (linkError) {
                        return !linkError.IsMedia;
                    });

                //Are we ordering the results?
                var orderedData = params.sorting() ?
                    $filter('orderBy')(filteredData, params.orderBy()) :
                    filteredData;

                //Set totals and page counts
                params.total(orderedData.length);
                var pagedResults = orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count());
                $defer.resolve(pagedResults);
            }
        })

    /*
    * Initial load function to set loaded state
    */
    $scope.initLoad = function () {
        if (!$scope.initialLoad) {
            //Get the available log dates to view log entries for.
            $scope.fetchLinkErrors()
                .then(function () { $scope.initialLoad = true; });
        }
    }

    $(function () {
        $scope.initLoad();
    });

});

/*
* LinkErrors API
* -----------------------------------------------------
* Resource to handle making requests to the backoffice API to handle CRUD operations
*/
angular.module("umbraco.resources").factory("LinkErrorsApi", function ($http) {
    return {
        //Get all redirects from the server
        getAll: function () {
            return $http.get("backoffice/LinkErrors/LinkErrorsApi/GetAll");
        },
        //Remove / Delete an existing redirect
        remove: function (id) {
            return $http.delete("backoffice/LinkErrors/LinkErrorsApi/Delete/" + id);
        },
        setRedirect: function (linkErrorId, nodeId, culture) {
            return $http.post("backoffice/LinkErrors/LinkErrorsApi/SetRedirect?linkErrorId=" + linkErrorId + "&nodeId=" + nodeId + "&culture=" + culture);
        },
        hide: function (linkErrorId, toggle) {
            return $http.post("backoffice/LinkErrors/LinkErrorsApi/Hide?id=" + linkErrorId + "&toggle=" + toggle);
        },
        //Clear cache
        clearCache: function () {
            return $http.post("backoffice/LinkErrors/LinkErrorsApi/ClearCache");
        }
    };
});
