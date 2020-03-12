angular.module("umbraco").controller("createRedirect.controller",
    function ($scope, languageResource, eventsService, contentResource) {

        $scope.dialogTreeApi = {};
        $scope.customTreeParams = "";

        $scope.submit = submit;
        $scope.close = close;

        $scope.allCultures = null;
        $scope.nodeCultures = null;

        $scope.searchInfo = {
            searchFromId: null,
            searchFromName: null,
            showSearch: false,
            results: [],
            selectedSearchResults: []
        };

        languageResource.getAll().then(function (data) {
            $scope.allCultures = data;
        });

        $scope.onTreeInit = function () {
            $scope.dialogTreeApi.callbacks.treeLoaded(treeLoadedHandler);
            $scope.dialogTreeApi.callbacks.treeNodeSelect(nodeSelectHandler);
            $scope.dialogTreeApi.callbacks.treeNodeExpanded(nodeExpandedHandler);
        }

        $scope.selectListViewNode = function (node) {
            node.selected = node.selected === true ? false : true;
            nodeSelectHandler({
                node: node
            });
        }

        $scope.hideSearch = function () {
            $scope.searchInfo.showSearch = false;
            $scope.searchInfo.searchFromId = null;
            $scope.searchInfo.searchFromName = null;
            $scope.searchInfo.results = [];
        }

        $scope.onSearchResults = function (results) {
            $scope.searchInfo.results = results;
            $scope.searchInfo.showSearch = true;
        }

        $scope.selectResult = function (evt, result) {
            result.selected = result.selected === true ? false : true;
            nodeSelectHandler({
                event: evt,
                node: result
            });
        }

        $scope.closeMiniListView = function () {
            $scope.miniListView = undefined;
        }

        var oneTimeTreeSync = {
            executed: false,
            treeReady: false,
            sync: function () {
                if (this.executed || !this.treeReady) {
                    return;
                }

                this.executed = true;

                $scope.dialogTreeApi.syncTree({
                    path: "-1",
                    tree: "content"
                });
            }
        };

        function treeLoadedHandler() {
            oneTimeTreeSync.treeReady = true;
            oneTimeTreeSync.sync();
        }

        function nodeSelectHandler(args) {
            if (args && args.event) {
                args.event.preventDefault();
                args.event.stopPropagation();
            }

            eventsService.emit("dialogs.linkPicker.select", args);

            if ($scope.model.selectedNode) {
                //un-select if there's a current one selected
                $scope.model.selectedNode.selected = false;
            }
            $scope.model.selectedCulture = null;
            $scope.nodeCultures = null;

            $scope.model.selectedNode = args.node;
            $scope.model.selectedNode.selected = true;

            contentResource.getById($scope.model.selectedNode.id).then(function (data) {
                var allCultures = data.urls.filter(x => x.isUrl).map(x => x.culture);
                var filteredCultures = allCultures.filter((item, index) => allCultures.indexOf(item) == index);
                $scope.nodeCultures = $scope.allCultures.filter(x => filteredCultures.find(culture => x.culture == culture) != null);
                if ($scope.nodeCultures.length > 0) {
                    $scope.model.selectedCulture = $scope.nodeCultures[0];
                }
            });
        }

        function nodeExpandedHandler(args) {
            // open mini list view for list views
            if (args.node.metaData.isContainer) {
                openMiniListView(args.node);
            }
        }

        function openMiniListView(node) {
            $scope.miniListView = node;
        }

        function close() {
            if ($scope.model && $scope.model.close) {
                $scope.model.close();
            }
        }

        function submit() {
            if ($scope.model && $scope.model.submit) {
                $scope.model.submit($scope.model);
            }
        }
    });
