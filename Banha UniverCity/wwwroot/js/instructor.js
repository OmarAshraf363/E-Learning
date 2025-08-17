function getSubmitions(id) {
    $.ajax({
        url: `/Instructor/Assinment/GetSubmitions`,
        type: 'GET',
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
function UploadAssinment(id, bindId) {
    $.ajax({
        url: "/instructor/assinment/UpSert",
        type: 'GET',
        data: { id: id, curriculumId: bindId },
        success: function (data) {
            $('#contentModal #modalContentBody').html(data);
            $('#contentModal').modal('show');
        },
        error: function () {
            alert('Failed to load submitions.');
        }

    })
}


//document.addEventListener("DOMContentLoaded", function () {
//    let indexWords = parseInt(document.querySelectorAll('#keyWordsContainer input').length);
//    let indexObjectives = parseInt(document.querySelectorAll('#objectivesContainer textarea').length);
//    let indexTopics = parseInt(document.querySelectorAll('#topicsContainer textarea').length);

//    document.getElementById('addKeyWord').addEventListener('click', function () {
//        const container = document.getElementById('keyWordsContainer');
//        const newField = document.createElement('div');
//        newField.classList.add('form-group', 'mb-3');
//        newField.innerHTML = `
//            <input name="KeyWords[${indexWords}].Name" class="form-control" placeholder="Enter word" required />
//            <span class="text-danger" data-valmsg-for="KeyWords[${indexWords}].Name"></span>`;
//        container.appendChild(newField);
//        indexWords++;
//    });

//    document.getElementById('addObjectiveBtn').addEventListener('click', function () {
//        const container = document.getElementById('objectivesContainer');
//        const newField = document.createElement('div');
//        newField.classList.add('form-group', 'mb-3');
//        newField.innerHTML = `
//            <textarea name="LearningObjectives[${indexObjectives}].Objective" class="form-control" placeholder="Enter learning objective" required></textarea>
//            <span class="text-danger" data-valmsg-for="LearningObjectives[${indexObjectives}].Objective"></span>`;
//        container.appendChild(newField);
//        indexObjectives++;
//    });

//    document.getElementById('addTopicBtn').addEventListener('click', function () {
//        const container = document.getElementById('topicsContainer');
//        const newField = document.createElement('div');
//        newField.classList.add('form-group', 'mb-3');
//        newField.innerHTML = `
//            <textarea name="TopicCovereds[${indexTopics}].Topic" class="form-control" placeholder="Enter topic" required></textarea>
//            <span class="text-danger" data-valmsg-for="TopicCovereds[${indexTopics}].Topic"></span>`;
//        container.appendChild(newField);
//        indexTopics++;
//    });
//});
