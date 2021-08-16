angular.module("umbraco").controller("LinkErrors.DetailController",
    function ($scope) {

        var vm = this;
        vm.close = close;

        function close() {
            if ($scope.model && $scope.model.close) {
                $scope.model.close();
            }
        }

    });