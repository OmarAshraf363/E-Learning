function getSpacifcCourses(id) {
    let btns = document.querySelectorAll(".category-list ul .btn");

    btns.forEach(btn => btn.classList.remove("active"));

    let selectedBtn = document.getElementById(`btn-${id}`);
    if (selectedBtn) {
        selectedBtn.classList.add("active");
    }

    let actionUrl = `Customer/Home/GetByDepTId`;

    $.ajax({
        url: actionUrl,
        type: 'GET',
        data: { id: id || 0 },
        success: function (partialHtml) {
            $('#specific').html(partialHtml);
        },
        error: function (error) {
            console.error("Error fetching courses:", error);
            $('#specific').html('<div class="alert alert-danger">Error loading courses.</div>');
        }
    });
}

function openModalWhenHover(courseID, sectionId) {
    const courseElement = document.getElementById(`modal-${sectionId}-${courseID}`);
    const courseCard = document.querySelector(`#${sectionId} #card-${courseID}`);

    // Get the course card position relative to the window
    const rect = courseCard.getBoundingClientRect();
    const isNearRightEdge = window.innerWidth - rect.right < 400; // 320px for the modal width

    // Adjust modal position based on the course card's position
    if (isNearRightEdge) {
        courseElement.classList.add('modal-left');
        courseElement.classList.remove('modal-right');
    } else {
        courseElement.classList.add('modal-right');
        courseElement.classList.remove('modal-left');
    }

    courseElement.style.display = 'block';
}

function closeModal(courseID, sectionId) {
    const courseElement = document.getElementById(`modal-${sectionId}-${courseID}`);
    courseElement.style.display = 'none';
}


function openModal(id, controllerName, modalname, actionName, areaName) {
    let url;
    if (id === null || id === undefined) {
        url = `/${areaName}/${controllerName}/${actionName}`;
    } else {
        url = `/${areaName}/${controllerName}/${actionName}/${id}`;
    }
    $.ajax({
        url: url,
        type: 'GET',
        success: function (data) {
            console.log(data)
            $(`#${modalname} .modal-content`).html(data);
            $(`#${modalname}`).modal('show');
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
function goTo(area, conroller, action, id) {
    let url = ``
    if (id) {
        url = `/${area}/${conroller}/${action}/${id}`
    } else {
        url = `/${area}/${conroller}/${action}`
    }
    window.location.href = url
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
                    } else {

                    let span = document.querySelector(`.glb`)
                    span.innerHTML = data.nameErrors[fieldName].join("<br>");
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
//api to getObjectives for one course
function getObjectives(id, num) {
    $.ajax({
        url: `/Customer/Home/GetObjectives`,
        type: "GET",
        data: { id: id },
        success: function (data) {
            console.log(data)

            document.querySelector(`#btn-${num}`).classList.add("btnactinve");
            $("#selected").empty()
            let content = ``
            data.forEach(function (e) {

                content += `   <li class="d-flex justify-content-start align-items-baseline gap-1">

                                           <i class="fas fa-check-circle text-success me-2"></i>

                                           <p class="m-1"> ${e.objective
                    }</p>


                                        </li>`
            })
            $("#selected").append(content)

        }

    })
}
//api to getObjectives for one course

function GetTopics(id, num) {
    $.ajax({
        url: `/Customer/Home/GetTopics`,
        type: "GET",
        data: { id: id },
        success: function (data) {
            console.log(data)
            document.querySelectorAll("ul .btn").forEach((btn) => {
                btn.classList.remove("btnactinve");
            });
            document.querySelector(`#btn-${num}`).classList.add("btnactinve");
            $("#selected").empty()
            let content = ``
            data.forEach(function (e) {

                content += `   <li class="d-flex justify-content-start align-items-baseline gap-1">

                                           <i class="fas fa-check-circle text-success me-2"></i>

                                           <p class="m-1"> ${e.topic
                    }</p>


                                        </li>`
            })
            $("#selected").append(content)

        }

    })
}


function FilterFeedback(star) {

}

function toggleHeader() {
    const header = document.querySelector('header')
    header.style.overflow = 'visible'
    header.style.height='300px'
}
const header = document.querySelector('header')
console.log(`headwr width is ${header.getBoundingClientRect().width}`)