var studentDetailService = studentDetailService || {};

studentDetailService.gridService = (function () {

    var privateMembers = {
        initializeControls: function () {
            $(function () {

                privateMembers.setGrid('tblExistingQualifications');
                privateMembers.setGrid('tblRejectedQualifications');

                AppGridSettings.PendingGrid = privateMembers.setPendingGrid('tblPendingQualifications');

            });
        },

        refreshPendingGrid: function () {

            //refresh pending Grid
            if (AppGridSettings.PendingGrid != undefined && AppGridSettings.PendingGrid != null) {

                AppGridSettings.PendingGridCurrentPage = AppGridSettings.PendingGrid.page.info().page;
            }

            $.ajax({
                url: window.AppUrlSettings.PendingUrl + '/' + $("#StudentId").val(),
                cache: false

            })
         .done(function (response, textStatus, jqXHR) {
             $("#dvpendingQual").html(response);

             AppGridSettings.PendingGrid = privateMembers.setPendingGrid('tblPendingQualifications');
              
             if (AppGridSettings.PendingGrid.page.info().pages > AppGridSettings.PendingGridCurrentPage) {
                 AppGridSettings.PendingGrid.page(AppGridSettings.PendingGridCurrentPage).draw('page');
             } else {
                 AppGridSettings.PendingGrid.page(AppGridSettings.PendingGridCurrentPage - 1).draw('page');
             }
             ajaxindicatorstop();

         });


        },

        refreshExistingGrid: function () {

            //refresh Existing Grid

            $.ajax({
                url: window.AppUrlSettings.ExistingUrl + '/' + $("#StudentId").val(),
                cache: false

            })
            .done(function (response, textStatus, jqXHR) {
                $("#dvExistingnQual").html(response);
                privateMembers.setGrid('tblExistingQualifications');
                ajaxindicatorstop();


            });

        },

        refreshRejectedGrid: function () {
            //refresh Rejected Grid

            $.ajax({
                url: window.AppUrlSettings.RejectedUrl + '/' + $("#StudentId").val(),
                cache: false

            })
            .done(function (response, textStatus, jqXHR) {
                $("#dvRejectedQual").html(response);

                privateMembers.setGrid('tblRejectedQualifications');
                ajaxindicatorstop();
            });
        },

        setGrid: function (tableId) {
            var dt = $('#' + tableId).DataTable({
                searching: false,
                paging: true,
                bLengthChange: false,
                language: {
                    emptyTable: AppMessages.noRecordMessage
                }
            });

            return dt;

        },

        setPendingGrid: function (tableId) {
            var dt = $('#' + tableId).DataTable({
                searching: false,
                paging: true,
                bLengthChange: false,
                columnDefs: [{ "orderable": false, "targets": [5, 6] }],
                language: {
                    emptyTable: AppMessages.noRecordMessage
                }
            });

            return dt;

        }
    }

    return {
        init: function () {
            privateMembers.initializeControls();

        },

        refreshPendingGrid: function () {
            privateMembers.refreshPendingGrid();
        },

        approveQualification: function (qualificationId) {


            ajaxindicatorstart('Processing.. please wait..');

            $.getJSON(window.AppUrlSettings.ApproveUrl + "?id=" + qualificationId + '&studentId=' + $("#StudentId").val(), function (result) {
                if (result.Status) {
                    showSuccessMessage(result.Message);
                    privateMembers.refreshPendingGrid();
                    privateMembers.refreshExistingGrid();

                } else {
                    showErrorMessage(result.Message);
                }

            },

            function () {

                logErrorMessage('Error in Approve : ' + qualificationId);
            })
            .always(function () {
                ajaxindicatorstop();
            });

        },

        openRejectPopUp: function (qualificationId) {
            $("#rejectedStudentQualificationId").val(qualificationId);
            $("#rejectRemark").val('');
            $('#rejectModal').modal('show');
        },

        rejectQualification: function () {

            if ($("#rejectRemark").val().length == 0) {

                showErrorMessage(AppMessages.RejectMessage);
            }
            else {

                ajaxindicatorstart('Processing.. please wait..');

                $.post(window.AppUrlSettings.RejectUrl,
                    {
                        id: $("#rejectedStudentQualificationId").val(),
                        studentId: $("#StudentId").val(),
                        remark: $("#rejectRemark").val()
                    })
                .done(function (result) {
                    if (result.Status) {
                        showSuccessMessage(result.Message);
                        privateMembers.refreshPendingGrid();
                        privateMembers.refreshRejectedGrid();
                        $('#rejectModal').modal('hide');

                    } else {
                        if (result.Message == undefined || result.Message.length == 0) {
                            showErrorMessage(AppMessages.InternalServerError);

                        } else {
                            showErrorMessage(result.Message);
                        }
                    }

                })
                    .fail(function () {

                        logErrorMessage('Error in Reject : ' + qualificationId);
                    })
                .always(function () {
                    ajaxindicatorstop();
                });
            }
        }

    }
}
)();




