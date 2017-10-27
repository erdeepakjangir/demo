var dashboardService = dashboardService || {};

dashboardService.SearchQualification = (function () {

    var privateMembers = {
        initializeControls: function () {
            $(function () {

                //set chosen
                $(".chosen-select").chosen({ no_results_text: "Oops, nothing found!" });

                //disable  when faculty
                if ($('#SearchQualification_FacultyCode option').length == 1) {                  

                    $("#SearchQualification_FacultyCode option").attr("disabled", true).trigger("chosen:updated");
                    $('#FacultyCode.chosen-select').attr("disabled", true).trigger("chosen:updated");
                }
                //set Date format
                var startdate = new Date();
                startdate.setYear(startdate.getFullYear() - 10),

                $('.datetimepicker').datepicker({
                    startDate: startdate,
                    endDate: '+0d',
                    autoclose: true

                });



                //validate date
                $("#btnSearch").click(function (event) {

                    if ($("#SearchQualification_SubmittedFromDate").val().length > 0 && $("#SearchQualification_SubmittedToDate").val().length > 0) {

                        var fromDate = new Date($("#SearchQualification_SubmittedFromDate").val());
                        var toDate = new Date($("#SearchQualification_SubmittedToDate").val());
                        if (fromDate > toDate) {
                            showErrorMessage(AppMessages.ValidDateRanage);
                            event.preventDefault();
                            return false;
                        }
                    }


                });

                //Date formatting support for sorting
                $.fn.dataTable.moment('dd/MM/YYYY');


                $("#SearchQualification_StudentId").on('change', function () {
                    $("#SearchQualification_StudentId").val($("#SearchQualification_StudentId").val().trim())
                });

            });
        },

        setQualificationGrid: function () {

            var $dt = $("#tblQualifications").DataTable({
                searching: false,
                paging: true,
                bLengthChange: false,
                order: [[6, "desc"]],
                pageLength: defaultPageSize,
                language: {
                    emptyTable: AppMessages.noRecordMessage
                }
            });

            // Add the Export buttons to the toolbox

            var buttontype;
            (navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1) ? buttontype = 'csv' : buttontype = 'excelHtml5';

            var buttons = new $.fn.dataTable.Buttons($dt, {
                buttons: [buttontype],
                dom: {
                    button: {
                        tag: 'button',
                        className: 'btn-icon btn btn-save'
                    }
                },
            }).container().appendTo($('#dvButton').empty());

            //Set total
            //Removed as per #27
            //var statusCol = $dt.column(5).data();

            //$("#spTotalApproved").text(statusCol.filter(function (val) { return val == 'Approved' }).count());
            //$("#spTotalPending").text(statusCol.filter(function (val) { return val == 'Pending' }).count());
            //$("#spTotalRejected").text(statusCol.filter(function (val) { return val == 'Rejected' }).count());


        }
    }
    return {
        init: function () {
            privateMembers.initializeControls();

        },

        setQualificationGrid: function () { privateMembers.setQualificationGrid(); }
    }
}
)();



