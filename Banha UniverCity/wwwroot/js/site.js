

//to delete and show modal
function deleteIt(itemId, controler) {
    Swal.fire({

        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Your work has been saved",
                showConfirmButton: false,
                timer: 1500
            }).then(() => {

                window.location.href = `/Admin/${controler}/Delete/${itemId}`;

            });
        }
    });
}


function customDelete(itemId, url) {
    Swal.fire({

        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                position: "top-end",
                icon: "success",
                title: "Your work has been saved",
                showConfirmButton: false,
                timer: 1500
            }).then(() => {

                window.location.href = `${url}/${itemId}`;

            });
        }
    });
}


//to check validation if valid return modal success if not return erorrs 
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


//ajax , get data from /area/controller/action and put this data in modal 

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

function openModalWithBind(id, bindId, controllerName, modalname, actionName, areaName) {
    $.ajax({
        url: `/${areaName}/${controllerName}/${actionName}`,
        type: 'GET',
        data: {
            id: id || 0, bindId: bindId
        },
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


function openModalToAddQuestion(id, examId, controllerName, modalname, actionName, areaName) {
    let url;
    if (id === null || id === undefined) {
        url = `/${areaName}/${controllerName}/${actionName}`;
    } else {
        url = `/${areaName}/${controllerName}/${actionName}/${id}`;
    }
    $.ajax({
        url: url,
        type: 'GET',
        data: {
            id: id, examId: examId, outSide: true
        },
        success: function (data) {
            $(`#${modalname} .modal-content`).html(data);
            $(`#${modalname}`).modal('show');
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}





function openAddToCourse(courseId, modalName) {
    $.ajax({
        url: `/Instructor/Instructor/UpsertCourseCurriculum`,
        type: 'GET',
        data: { courseId: courseId },
        success: function (data) {
            // تعيين المحتوى الجديد داخل المودال
            $(`#${modalName} .modal-content`).html(data);
            // عرض المودال
            $(`#${modalName}`).modal('show');
        },
        error: function (xhr, status, error) {
            // طباعة الخطأ إذا فشل الطلب
            console.error('Error fetching the modal content:', error);
        }
    });
}


function openAddToCourseRefOrVideo(curriculumId, bindId, modalName, area, action, controller, fromCourse) {
    let actionUrl = '';

    if (modalName === 'AddVideoModal') {
        actionUrl = `/Instructor/Instructor/UpsertCourseVideo`;
    } else if (modalName === 'AddReferenceModal') {
        actionUrl = `/Instructor/Instructor/UpsertReference`;
    } else {
        actionUrl = `/${area}/${controller}/${action}`
    }

    $.ajax({
        url: actionUrl,
        type: 'GET',
        data: {
            id: bindId || 0,
            curriculumId: curriculumId,
            fromCourseArea: fromCourse || null
        },
        success: function (data) {
            $(`#${modalName} .modal-content`).html(data);
            $(`#${modalName}`).modal('show');

        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}



function openModalWithBindId(thisId, bindId, controller, modalName, action, area) {
    let actionUrl = `/${area}/${controller}/${action}`


    $.ajax({
        url: actionUrl,
        type: 'GET',
        data: {
            id: thisId || 0,
            courseId: bindId,

        },
        success: function (data) {

            $(`#${modalName} .modal-content`).html(data);
            $(`#${modalName}`).modal('show');

        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}


function openAssignmentModal(assignmentId) {
    $.ajax({
        url: `/instructor/assinment/GetAssignmentDetails`,
        type: 'GET',
        data: { id: assignmentId },
        success: function (data) {
            $('#assignmentDetailsContent').html(data);
            $('#assignmentDetailsModal').modal('show');
        },
        error: function () {
            alert('Failed to load assignment details.');
        }
    });
}

function openCourseCurriculum(courseId) {
    $.ajax({
        url: `/Instructor/Instructor/GetCourseCurriculum?courseId=${courseId}`,
        type: 'GET',
        success: function (data) {
            $('#curriculumModal .modal-body').html(data);
            $('#curriculumModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}

function openDetails(courseId, modalName) {




    // Fetch course details and load into the modal
    $.ajax({
        url: `/Student/DashBoard/CourseDetails`,
        type: 'GET',
        data: { id: courseId },
        success: function (result) {

            $(`#${modalName} #courseDetailsContent`).html(result);
            $(`#${modalName}`).modal('show');;
        },
        error: function () {
            modal.find('#courseDetailsContent').html('<p>Error loading course details. Please try again later.</p>');
        }
    });
};

//get curculum
function getContent(curculamId, divId) {
    $.ajax({

        url: `/Student/DashBoard/GetContent`,
        type: 'GET',
        data: { id: curculamId },
        success: function (data) {
            $(`#${divId}`).html(data)
        },
        error: function (erorr) {
            alert(erorr);
        }


    })
}

//get one video to show it 
function getVideo(id, divId) {
    $.ajax({

        url: `/Student/DashBoard/GetVideo`,
        type: 'GET',
        data: { id: id },
        success: function (data) {
            let videocontent = `<video   id="myVideo" width="100%" controls>
                        <source  src="${data.videoURL}" type="video/mp4">
                        Your browser does not support the video tag.
                    </video>
                    <p>${data.VideoTitle}</p>`
            $(`#${divId}`).html(videocontent)
        },
        error: function (erorr) {
            alert(erorr);
        }


    })

}





//back one bage from history

function getback() {
    window.history.back();
}





//when hover in course-card
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







//api to get courses in home page by spacifc category
function getSpacifcCourses(id) {
    let btns = document.querySelectorAll(".category-list ul .btn");

    btns.forEach(e => {
        btns.forEach(l => {
            l.classList.remove("active")
        })
    })
    let btn = document.getElementById(`btn-${id}`)
    btn.classList.add("active");
    let actionUrl = `Customer/Home/GetByDepTId`;

    $.ajax({
        url: actionUrl,
        type: 'GET',
        data: {
            id: id || 0,
        },
        success: function (data) {
            // Clear the current courses
            $('#specific').empty();
            console.log(data)

            let description = ` <div class="department-Description">
                    
                <p class="w-75">
                    ${data[0].departmentDescription}
                </p>
                </div>`

            $('#specific').html(description);

            // Loop through the fetched courses and render them
            data.forEach(function (course) {

                let objectivesHtml = '';
                course.learningObjectives.forEach(function (objective) {
                    objectivesHtml += `
 
                     <li class="d-flex justify-content-start align-items-baseline gap-1">

                                           <i class="fas fa-check-circle text-success me-2"></i>

                                           <p class="m-1"> ${objective}</p>


                                        </li>

                    `;
                });



                let courseHtml = `

              


            <div id="card-${course.courseID}" class="one-course" onclick="goTo('Customer','Home','Courses',${course.courseID})" onmouseover="openModalWhenHover(${course.courseID}, 'specific')" onmouseleave="closeModal(${course.courseID}, 'specific')">
                <div class="course-video mb-3">
                    <img src="/Covers/${course.imgCover}" alt="${course.courseName}" />
                </div>
                <div class="course-title">
                    <h5>${course.courseName}</h5>
                </div>
                <div class="w-75 instructor">
                    <h6>${course.instructor}</h6>
                </div>

                <!-- Rating -->
                <div class="rating">`;

                // Generate stars for the rating
                for (let i = 0; i < course.rate; i++) {
                    courseHtml += `<span class="fa fa-star"></span>`;
                }
                for (let i = 0; i < (5 - course.rate); i++) {
                    courseHtml += `<span class="fa fa-star-o"></span>`;
                }

                courseHtml += `
                </div>

                <!-- Price -->
                <div class="price">
                    <span>$${course.price}</span>
                </div>


                <div class="modal-test p-4" id="modal-specific-${course.courseID}" style="max-width: 700px; box-shadow: 0px 8px 20px rgba(0, 0, 0, 0.1); border-radius: 10px; background-color: #fff;">
                        <!-- Course Name Section -->
                        <div class="modal-course-header ">
                        <h3 class="modal-course-name mb-3">${course.courseName}</h3>
                            <h5>by <span>${course.instructor}e</span></h5>
                        </div>

                        <!-- Course Stats Section -->
                        <div class="modal-course-details d-flex justify-content-between mb-2">
                            <div class="course-videos">
                                <span><i class="fas fa-play-circle me-2"></i>${course.videosCount} Videos</span>
                            </div>

                            <div class="course-enrollments">
                                <span><i class="fas fa-users me-2"></i>${course.enrollmentsCount} Enrolled</span>
                            </div>
                        </div>

                        <!-- Course Description Section -->
                        <div class="modal-course-description mb-2">
                            <p> ${course.description}</p>
                        </div>

                        <!-- Course Topics and Objectives Section -->
                        <div class="modal-course-about ">

                            <div class="course-objectives ">

                                <ul class="list-unstyled">


                               ${objectivesHtml}

                                
                                </ul>
                            </div>
                        </div>

                        <!-- Course Call to Action -->
                        <div class="d-flex justify-content-center mt-2">
                            <button class="btn cus-btn px-5 py-2" style="border-radius: 5px; font-size: 1.1rem;">Add to Cart</button>
                        </div>
                    </div>




        `;

                $('#specific').append(courseHtml);
            });
        },

        error: function (error) {
            console.error("Error fetching courses:", error);
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

function goTo(area, conroller, action, id) {
    let url = ``
    if (id) {
        url = `/${area}/${conroller}/${action}/${id}`
    } else {
        url = `/${area}/${conroller}/${action}`
    }
    window.location.href = url
}





//window.addEventListener("scroll", function () {
//    var summary = document.getElementById("summary");
//    var summaryOffsetTop = summary.offsetTop; // Get the top position of the summary
//    var scrollPosition = window.scrollY || window.pageYOffset; // Get the current scroll position

//    // If the user has scrolled past the summary's top position, fix the position of the summary
//    if (scrollPosition > summaryOffsetTop-20) {
//        summary.classList.add("fixed");
//    } else {
//        summary.classList.remove("fixed");
//    }
//});





let currentQuestionIndex = 0;
let allQuestions = [];

// Fetch questions and render the first one
function loadQuestions(examId) {
    $.ajax({
        url: `/Student/DashBoard/GetQuestions?id=${examId}`,
        type: 'GET',
        success: function (data) {
            allQuestions = data; // Store all questions in a global variable
            if (allQuestions.length > 0) {
                renderSingleQuestion(currentQuestionIndex); // Render the first question
                $('#examModal').modal('show'); // Show modal
            }
        },
        error: function (error) {
            console.error("Error fetching questions", error);
        }
    });
}

// Function to render a single question based on the current index
function renderSingleQuestion(index) {
    const question = allQuestions[index];
    if (question) {
        $('#questionSection').html(`
            <div class="question-container" id="question-${index}">
                <h4>${question.questionText}</h4>
                <ul>
                    ${question.choices.map((choice, i) => `
                        <li>
                            <label>
                                <input type="radio" name="Choices-${index}" value="${choice}" /> ${choice}
                            </label>
                        </li>
                    `).join('')}
                </ul>
            </div>
        `);
    }
    updateNavigationButtons();
}

// Function to update the state of Next/Previous buttons
function updateNavigationButtons() {
    $('#nextButton').prop('disabled', currentQuestionIndex >= allQuestions.length - 1);
    $('#prevButton').prop('disabled', currentQuestionIndex <= 0);
}

// Handle Next button click
function goToNextQuestion() {
    if (currentQuestionIndex < allQuestions.length - 1) {
        currentQuestionIndex++;
        renderSingleQuestion(currentQuestionIndex);
    }
}

// Handle Previous button click
function goToPreviousQuestion() {
    if (currentQuestionIndex > 0) {
        currentQuestionIndex--;
        renderSingleQuestion(currentQuestionIndex);
    }
}



function testVideo(id) {
    const video = document.getElementById("myVideo");

    if (video) {
        video.addEventListener('timeupdate', function () {
            const currentTime = video.currentTime;
            const duration = video.duration;
            const percentageWatched = (currentTime / duration) * 100;

            if (percentageWatched > 25) {
                // ارسال طلب AJAX لجلب الأسئلة
                loadQuestions(id)

                // إزالة الحدث لمنع التكرار
                video.removeEventListener('timeupdate', arguments.callee);
            }
        });
    } else {
        console.log("No video element found");
    }
}


function filter(word) {
    $.ajax({
        url: `/Customer/Home/Courses`,
        type: 'GET',
        data: { id: assignmentId },
        success: function (data) {
            $('#assignmentDetailsContent').html(data);
            $('#assignmentDetailsModal').modal('show');
        },
        error: function () {
            alert('Failed to load assignment details.');
        }
    });
}




//function showReaction(id) {
//    let reactElement = document.getElementById(`react-part-${id}`);
//    let reactionMenu = document.getElementById(`reaction-menu-${id}`);

//    if (reactionMenu.style.display == "none") {
//        reactionMenu.style.display = "block"
//    } else if (reactionMenu.style.display == "block") {
//        reactionMenu.style.display = "none";
//    }
//    //reactElement.onmouseleave = function () {
//    //    reactionMenu.style.display = "none";
//    //}


//    // Toggle the menu visibility
//    reactionMenu.style.display = reactionMenu.style.display === "none" ? "flex" : "none";
//}



function addReaction(postId, reactionType, targetType) {
    $.ajax({
        url: `/Customer/Community/AddReaction`,
        type: 'POST',
        data: {
            id: postId,
            typeReact: reactionType,
            targetType: targetType,
        },
        success: function (data) {
            console.log(data);

            let react = document.getElementById(`react-place-${postId}`);
            let reactionCountElement = document.getElementById(`reaction-count-${postId}`);

            if (data.valid && data.state === 'Add') {
                console.log(`Reaction "${data.typeReact}" added successfully.`);
                react.classList.remove('fa-regular');
                react.classList.add('fa-solid');
            } else if (data.valid && data.state === 'Remove') {
                console.log(`Reaction removed successfully.`);
                react.classList.remove('fa-solid');
                react.classList.add('fa-regular');
            } else {
                console.error('Failed to update reaction.');
            }

            // Fetch the updated count and dynamically update the UI
            $.ajax({
                url: `/Customer/Community/CountReaction`,
                type: 'GET',
                data: { id: postId },
                success: function (result) {
                    console.log(`Updated reaction count: ${result.count}`);
                    reactionCountElement.innerHTML = " "
                    reactionCountElement.innerHTML = result.count
                },
                error: function (error) {
                    console.error("Failed to fetch updated reaction count.", error);
                }
            });
        },
        error: function (error) {
            console.error("Error updating reaction.", error);
        }
    });
}

function addReactionForComment(id) {
    const reactBtn = document.getElementById(`react-btn-${id}`);
    const reactIcon = document.getElementById(`react-icon-${id}`);
    const reactionCountElement = document.getElementById(`reaction-count-${id}`);

    $.ajax({
        url: `/Customer/Community/addReactInComment`,
        type: 'POST',
        data: { id: id },
        success: function (data) {
            if (data.valid) {
                if (data.state === 'Add') {
                    reactIcon.classList.remove('fa-regular');
                    reactIcon.classList.add('fa-solid');
                } else if (data.state === 'Remove') {
                    reactIcon.classList.remove('fa-solid');
                    reactIcon.classList.add('fa-regular');
                }

                // Update reaction count
                $.ajax({
                    url: `/Customer/Community/CountReactionInComment`,
                    type: 'GET',
                    data: { id: id },
                    success: function (result) {
                        reactionCountElement.innerHTML = " ";
                        console.log(reactionCountElement)
                        reactionCountElement.innerHTML = result.count
                    },
                    error: function (error) {
                        console.error("Failed to fetch updated reaction count.", error);
                    }
                });
            } else {
                alert("Failed to update reaction.");
            }
        },
        error: function (error) {
            console.error("Error updating reaction.", error);
        }
    });
}

function toggleReplyBox(id) {
    const replyBox = document.getElementById(`reply-box-${id}`);
    if (replyBox.style.display === 'none' || replyBox.style.display === '') {
        replyBox.style.display = 'block';
    } else {
        replyBox.style.display = 'none';
    }
}

function submitReply(commentId,postId) {
    const replyText = document.getElementById(`reply-text-${commentId}`).value;

    if (!replyText.trim()) {
        alert("Reply content cannot be empty.");
        return;
    }

    $.ajax({
        url: '/Customer/Community/AddReply',
        type: 'POST',
        data: {
            commentId: commentId,
            replyContent: replyText,
                postId: postId,
        },
        success: function (data) {
            if (data.valid) {
                alert(data.message);

                // Optionally reload the page or dynamically update the UI
                location.reload();
            } else {
                alert(data.message);
            }
        },
        error: function (error) {
            console.error("Error submitting reply:", error);
        }
    });
}


function showReaction(id,actionName) {
    const modalElement = document.getElementById(`modal-${id}`);
    const spanElement = document.getElementById(`Reactions-${id}`);

    if (!modalElement || !spanElement) {
        console.error('Modal or Span element not found for id:', id);
        return;
    }

    $.ajax({
        url: `/Customer/Community/${actionName}`,
        type: 'GET',
        data: { id: id },
        beforeSend: function () {
            spanElement.textContent = 'Loading...'; // Show loading indicator
        },
        success: function (data) {
            modalElement.innerHTML = data;

            // Toggle visibility of the modal
            modalElement.style.display = modalElement.style.display === "block" ? "none" : "block";
        },
        error: function (xhr, status, error) {
            console.error('Error fetching reaction details:', error);
            spanElement.textContent = 'Error loading reactions';
        },
        complete: function () {
            spanElement.textContent = 'Reactions'; // Reset text after completion
        }
    });
}



function showHandelMenu(id) {
    const menu = document.querySelector(`.post .handel-${id} .handel-menu`);
    if (menu.style.display === "block") {
        menu.style.display = "none";
    } else {
        menu.style.display = "block";
    }

}




