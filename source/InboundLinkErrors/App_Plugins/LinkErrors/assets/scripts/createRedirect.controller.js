angular.module("umbraco").controller("createRedirect.controller",
    function ($scope, eventsService, languageResource) {

        $scope.dialogTreeApi = {};

        $scope.selectedCulture = null;
        $scope.cultures = [];

        languageResource.getAll().then(function (data) {
            $scope.cultures = data;
            $scope.selectedCulture = $scope.cultures[0];
        });

        $scope.onTreeInit = function () {
            $scope.dialogTreeApi.callbacks.treeLoaded(treeLoadedHandler);
            $scope.dialogTreeApi.callbacks.treeNodeSelect(nodeSelectHandler);
            $scope.dialogTreeApi.callbacks.treeNodeExpanded(nodeExpandedHandler);
        }

        var oneTimeTreeSync = {
            executed: false,
            treeReady: false,
            sync: function () {
                if (this.executed || !this.treeReady || !($scope.model.target && $scope.model.target.path)) {
                    return;
                }

                this.executed = true;

                $scope.dialogTreeApi.syncTree({
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

            if ($scope.currentNode) {
                //un-select if there's a current one selected
                $scope.currentNode.selected = false;
            }

            console.log("selected node!");
            $scope.currentNode = args.node;
            $scope.currentNode.selected = true;
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
    });
