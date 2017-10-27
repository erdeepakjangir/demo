var reportService = reportService || {};

reportService.Summary = (function () {

    var privateMembers = {
        initializeControls: function () {

            $("input[name='chkType']").click(function () {
                if ($("#chkYear").is(":checked")) {
                    $("#yearView").show();
                    $("#dateView").hide();
                    $("#SearchQualification_SubmittdFromDate").val('');
                    $("#SearchQualification_SubmittdToDate").val('');
                }


                if ($("#chkDate").is(":checked")) {
                    $("#dateView").show();
                    $("#yearView").hide();
                    $("#SearchQualification_Year").val('');
                }
            });

            $(".chosen-select").chosen({ no_results_text: "Oops, nothing found!" });

            var startdate = new Date();
            startdate.setYear(startdate.getFullYear() - 10),

            $('.datetimepicker').datepicker({
                startDate: startdate,
                endDate: '+0d',
                autoclose: true

            });

            $("#btnSearch").click(function (event) {
                var isValidForm = true;

                if ($("#chkYear").is(":checked")) {

                    if ($("#SearchQualification_Year").val().length == 0) {
                        showErrorMessage(AppMessages.YearRequired);
                        isValidForm = false;
                    }

                }
                if ($("#chkDate").is(":checked")) {
                    if (($("#SearchQualification_SubmittedFromDate").val().length == 0 || $("#SearchQualification_SubmittedToDate").val().length == 0)) {
                        showErrorMessage(AppMessages.DateRanageRequired);

                        isValidForm = false;
                    }
                    else if ($("#SearchQualification_SubmittedFromDate").val().length > 0 && $("#SearchQualification_SubmittedToDate").val().length > 0) {

                        var fromDate = new Date($("#SearchQualification_SubmittedFromDate").val());
                        var toDate = new Date($("#SearchQualification_SubmittedToDate").val());
                        if (fromDate > toDate) {
                            showErrorMessage(AppMessages.ValidDateRanage);

                            isValidForm = false;
                        }
                    }
                }

                if (!isValidForm) {
                    event.preventDefault();
                    return false;
                }
            });

            $("#btnSearch").submit();



           

        },

        getFacultyView: function (facultyCode) {
            $('#lnkBack').show();

            $('#SearchQualification_FacultyCode').val(facultyCode);

            $("#btnSearch").click();
        },

        getPlannerView: function () {
            $('#lnkBack').hide();
            $('#SearchQualification_FacultyCode').val('');
            $("#btnSearch").click();
        },

        attachEvents: function () {

        },


    };

    return {
        init: function () {
            privateMembers.initializeControls();
            privateMembers.attachEvents();
            privateMembers.IsAdminView = true;
        },


        loadFacultyView: function (facultyCode) {
            privateMembers.getFacultyView(facultyCode);
             privateMembers.IsAdminView = true;
        },

        loadPlannerView: function () {
            privateMembers.getPlannerView();
             privateMembers.IsAdminView = false;
        }

    }
}
)();
