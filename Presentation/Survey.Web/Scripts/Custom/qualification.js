var qualificationService = qualificationService || {};

qualificationService.AddQualification = (function () {

    var privateMembers = {
        initializeControls: function () {
            $(function () {

                $("#tblRejectedQualifications").DataTable({  //#tblApprovedQualifications,
                    searching: false,
                    paging: true,
                    bLengthChange: false,
                    language: {
                        infoEmpty: AppMessages.noRecordMessage
                    }
                });

                AppGridSettings.PendingGrid = qualificationService.Common.setPendingGrid();

                $('#qualificationTab a.active').tab('show')

                $(".chosen-select").chosen({ no_results_text: "Oops, nothing found!" });

            });
        },

        attachEvents: function () {

            $("#AddQualification_QualificationTypeCode").off('change').on('change', function () {

                var qualificationCode = $(this).val();
                if (qualificationCode.length > 0) {
                    qualificationService.Common.getDropdownValues(qualificationCode, $("#dvAddQualification #AddQualification_SubjectCode"), $("#dvAddQualification #AddQualification_Result"));
                }

            });


            $("#btnAddQualification").click(function (e) {

                var myform = $("#frmAddQualification");
                myform.validate();

                if (!$(myform).valid()) {
                    return false;
                }

                if ($("#AddQualification_Year").val().length > 0 && $("#AddQualification_QualificationTypeCode").val().length > 0
                    && $("#AddQualification_SubjectCode").val().length > 0 && $("#AddQualification_Result").val().length > 0
                    && $("#AddQualification_SittingCode").val().length > 0) {


                    var qualificationData = new FormData($('form#frmAddQualification')[0]);

                    var fileUpload = $("#AddQualification_Certificate").get(0);
                    var files = fileUpload.files;

                    if (files != null && files.length > 0) {

                        if (files[0].size > maxCertificateSize) {
                            showErrorMessage(AppMessages.MaxFileSize);
                            return false;
                        }
                        qualificationData.append('certificate', files[0]);
                    }

                    ajaxindicatorstart('Processing.. please wait..');

                    $.ajax({
                        url: AppUrlSettings.AddQualificationUrl,
                        type: "POST",
                        contentType: false, // Not to set any content header
                        processData: false, // Not to process data
                        data: qualificationData
                    })
        .done(function (response, textStatus, jqXHR) {
            if (response != null) {
                //clear drop down


                if (response.Status == true) {

                    showSuccessMessage(response.Message);
                    //refresh Grid
                    qualificationService.Common.refreshPendingGrid();
                    privateMembers.clearDropdown();
                }
                else {
                    showErrorMessage(response.Message);
                }
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {

            //clear drop down
            // privateMembers.clearDropdown();

            showErrorMessage(errorThrown);
        })
        .always(function (result, textStatus, jqXHR) {

            ajaxindicatorstop();
        });


                }
            });


            $("#AddQualification_Certificate").on('change', function (event) {

                var fileName = $("#AddQualification_Certificate").val();


                $('#dvfileName').empty().html($('<p/>').text(fileName.slice(fileName.lastIndexOf('\\') + 1))).show();
                $("#dvRemove").show();
                $('#dvFileUpload').hide();
            });

            $("#btnAddQualification_Remove").on('click', function (event) {
                $("#AddQualification_Certificate").val(null);
                $("#dvRemove").hide();
                $('#dvFileUpload').show();
                $('#dvfileName').empty();
                $("#dvfileName").empty().hide();


            });

        },

        clearDropdown: function () {

            $("#AddQualification_Year").val('');

            $("#AddQualification_QualificationTypeCode").val('');

            $("#AddQualification_SubjectCode ").find("option:not(:first)").remove();

            $("#AddQualification_Result").find("option:not(:first)").remove();

            $("#AddQualification_SittingCode").val('');

            $("#AddQualificatio_CertificateId").val('')

            $("#btnAddQualification_Remove").click(); //reset fields
        }

    };

    return {
        init: function () {
            privateMembers.initializeControls();
            privateMembers.attachEvents();
        }

    }
}
)();


qualificationService.EditQualification = (function () {

    var privateMembers = {
        currentGridPage: 0,
        initializeControls: function () {

        },

        attachEvents: function () {

            $("#editModal #QualificationTypeCode").off('change').on('change', function () {

                var qualificationCode = $(this).val();
                if (qualificationCode.length > 0) {
                    qualificationService.Common.getDropdownValues(qualificationCode, $("#editModal #SubjectCode"), $("#editModal #Result"));
                }

            });

            $('#EditQualification_Certificate').off('click').on('click', function () {
                $('#editQualification_Certificate').click();
            });

            $("#editQualification_Certificate").off('change').on('change', function () {

                var fileUpload = $("#editQualification_Certificate").get(0);
                var files = fileUpload.files;

                if (files != null && files.length > 0) {

                    if (files[0].size > maxCertificateSize) {
                        showErrorMessage(AppMessages.MaxFileSize);
                        return false;
                    }
                    ajaxindicatorstart('Processing.. please wait..');

                    var fileName = $("#editQualification_Certificate").val();

                    $('#dvFileNameEdit').empty().html($('<p/>').text(fileName.slice(fileName.lastIndexOf('\\') + 1))).show();



                    var qualificationData = new FormData();
                    qualificationData.append('certificate', files[0]);

                    $.ajax({
                        url: AppUrlSettings.UploadCertificateUrl,
                        type: "POST",
                        contentType: false, // Not to set any content header
                        processData: false, // Not to process data
                        data: qualificationData
                    })
        .done(function (response, textStatus, jqXHR) {
            if (response != null) {
                $("#dvRemoveEdit").show();
                if (response.Status == true) {

                    //refresh Grid
                    $("form#editform #CertificateId").val(response.CertificateId);

                    qualificationService.Common.refreshPendingGrid();
                }
                else {
                    showErrorMessage(response.Message);
                }
            }
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            showErrorMessage(errorThrown);
        })
        .always(function (result, textStatus, jqXHR) {
            ajaxindicatorstop();
        });
                }
            });


            $("#btnEditQualification_Remove").on('click', function (event) {
                $("#EditQualification_Certificate").val(null);
                $("form#editform #CertificateId").val('');
                $('#dvFileNameEdit').empty();
                $("#dvRemoveEdit").hide();
                $('#dvFileUploadEdit').show();
                $("#lnkViewCertificate").hide();
            });
        }
    };

    return {
        init: function () {
            privateMembers.initializeControls();
            privateMembers.attachEvents();
        },
        disableSubmit: function () {
            $('#editModal #btnEditSave').addClass('disabled');
            $("#editModal div.success-msg").empty();
        }
        ,
        loadEditPopup: function (url) {
            $('#editModal .modal-body').empty();
            $.ajax({
                url: url,
                cache: false

            })
        .done(function (response, textStatus, jqXHR) {

            $('#editModal .modal-body').html(response);
        });
        },

        loadComplete: function complete() {
            $("form#editform").each(function () {
                $.data($(this)[0], 'validator', false);
            });
            $.validator.unobtrusive.parse("form#editform");
        }
    }
}
)();


qualificationService.DeleteQualification = function (qualificationId) {

    ajaxindicatorstart('Processing.. please wait..');

    $.getJSON(AppUrlSettings.DeleteQualificationUrl + "?id=" + qualificationId, function (result) {
        if (result.Status) {
            showSuccessMessage(result.Message);
            qualificationService.Common.refreshPendingGrid();

        }
        else {
            showErrorMessage(result.Message);
        }
    },

    function () {

        logErrorMessage('Error in delete : ' + qualificationId);
    })
    .always(function () {
        ajaxindicatorstop();
    });
};



qualificationService.Common = (function () {
    return {
        refreshPendingGrid: function () {
            if (AppGridSettings.PendingGrid != undefined && AppGridSettings.PendingGrid != null) {
                AppGridSettings.PendingGridCurrentPage = AppGridSettings.PendingGrid.page.info().page;
            }

            $.ajax({
                url: window.AppUrlSettings.PendingQualificationUrl,
                cache: false

            })
       .done(function (response, textStatus, jqXHR) {
           $(".tab-pane#pendingQual").html(response);

           AppGridSettings.PendingGrid = qualificationService.Common.setPendingGrid();
           //set current page
           if (AppGridSettings.PendingGrid.page.info().pages > AppGridSettings.PendingGridCurrentPage) {
               AppGridSettings.PendingGrid.page(AppGridSettings.PendingGridCurrentPage).draw('page');
           }
           else {
               AppGridSettings.PendingGrid.page(AppGridSettings.PendingGridCurrentPage - 1).draw('page');
           }

           ajaxindicatorstop();
           //alert(6);
       });

        },

        getDropdownValues: function (qualificationCode, ddlSubjectElement, ddlResultElement) {

            ajaxindicatorstart();

            $.getJSON(window.AppUrlSettings.GetSubjectAndResultUrl + "?qualificationCode=" + qualificationCode, function (result) {
                ajaxindicatorstop();

                $(ddlSubjectElement).find("option:not(:first)").remove();

                $.each(result.Subjects, function (index, item) {
                    $(ddlSubjectElement).append("<option value ='" + item.SubjectCode + "'>" + item.SubjectTitle + "</option>");
                });

                $(ddlResultElement).find("option:not(:first)").remove();

                $.each(result.Results, function (index, item) {
                    $(ddlResultElement).append("<option value ='" + item.Result + "'>" + item.Result + "</option>");
                });
            })
            .always(function () {
                ajaxindicatorstop();
            });
        },

        setPendingGrid: function () {

            return $("#tblPendingQualifications").DataTable({
                searching: false,
                paging: true,
                bLengthChange: false,
                bAutoWidth: false,
                //responsive: true,
                columnDefs: [{ "orderable": false, "targets": [5] }],
                language: {
                    emptyTable: AppMessages.noRecordMessage
                }
            });
        }
    }
}

)();
