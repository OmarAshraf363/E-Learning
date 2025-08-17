


function UploadAssinmentSoloution(id) {
    $.ajax({
        url: `/student/studentassinment/UploadSoluation`,
        type: "GET",
        data: { id: id },
        success: function (data) {
            $('#contentModal #modalContentBody').html(data);
            $('#contentModal').modal('show');
        },
        error: function () {
            alert('Failed to load submitions.');
        }
    })
}

function QuickAccessToSpacifcCourse(id) {
    $.ajax({
        url: `/student/dashboard/Quickaccess`,
        type: 'GET',
        data: { id: id },
        success: function (data) {
            $('#contentModal #modalContentBody').html(data);
            $('#contentModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}

function UpsertFeedBack(id, courseId, curriculumId) {
    $.ajax({
        url: `/student/dashboard/UpsertFeedback`,
        type: "GET",
        data: { id: id, courseId: courseId, curriculumId: curriculumId },
        success: function (data) {
            $('#contentModal #modalContentBody').html(data);
            $('#contentModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    })
}

function allConfirm(id) {
    let form = document.getElementById(id);

    fetch(form.action, {
        method: form.method,
        body: new FormData(form),
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    }).then(response => response.json())
        .then(data => {
            if (!data.isvalid) {
                console.log(data)
                form.querySelectorAll("span.text-danger").forEach(span => {
                    span.innerHTML = "";
                });

                let field;
                for (let fieldName in data.nameErrors) {
                    /* field = document.querySelector(`#${id}#${fieldName}`);*/
                    field = document.getElementById(`${fieldName}`)
                    if (field) {
                        let span = field.nextElementSibling;
                        if (span && span.classList.contains('text-danger')) {
                            console.log(data.nameErrors[fieldName])
                            span.innerHTML = data.nameErrors[fieldName].join("<br>");
                        }
                    }


                }
            } else {

                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Your work has been Done",
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    location.reload()
                });
            }
        });
}