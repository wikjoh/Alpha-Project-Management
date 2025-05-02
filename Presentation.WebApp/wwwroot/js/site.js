document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150

    // handle dropdown menus
    const dropdowns = document.querySelectorAll('[data-type="dropdown"]')

    document.addEventListener('click', (e) => {
        let clickedDropdown = null

        dropdowns.forEach(dropdown => {
            const targetId = dropdown.getAttribute('data-target')
            const targetElement = document.querySelector(targetId)

            if (dropdown.contains(e.target)) {
                clickedDropdown = targetElement

                document.querySelectorAll('.dropdown-show').forEach(openDropdown => {
                    if (openDropdown !== targetElement) {
                        openDropdown.classList.remove('dropdown-show')
                    }
                })

                targetElement.classList.toggle('dropdown-show')
            }
        })

        if (!clickedDropdown && !e.target.closest('.dropdown')) {
            document.querySelectorAll('.dropdown-show').forEach(openDropdown => {
                openDropdown.classList.remove('dropdown-show')
            })
        }
    })

    // open modal (and populate if needed)
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)

            const clientId = button.getAttribute('data-client-id')
            if (clientId) {
                try {
                    const response = await fetch(`/admin/getClient/id/${clientId}`);
                    if (response.ok) {
                        const clientData = await response.json();
                        populateEditClientModal(clientData, modal);
                    }
                } catch (error) {
                    console.error('Error fetching client data: ', error);
                }
            }

            const memberId = button.getAttribute('data-user-id')
            if (memberId) {
                try {
                    const response = await fetch(`/admin/getMember/id/${memberId}`);
                    if (response.ok) {
                        const memberData = await response.json();
                        populateEditMemberModal(memberData, modal)
                    }
                } catch (error) {
                    console.error('Error fetching member data: ', error);
                }
            }

            const projectId = button.getAttribute('data-project-id')
            if (projectId) {
                try {
                    const response = await fetch(`/getProject/id/${projectId}`);
                    if (response.ok) {
                        const projectData = await response.json();
                        populateEditProjectModal(projectData, modal)

                        // dispatch event for tags.js
                        document.dispatchEvent(new CustomEvent('projectDataLoaded', {
                            detail: { projectData }
                        }));
                    }
                } catch (error) {
                    console.error('Error fetching project data: ', error);
                }
            }

            if (modal)
                modal.style.display = 'flex';
        })
    })

    // close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal')
            if (modal) {
                modal.style.display = 'none'

                modal.querySelectorAll('form').forEach(form => {
                    form.reset()

                    const imagePreview = form.querySelector('.image-preview')
                    if (imagePreview)
                        imagePreview.src = ''

                    const imagePreviewer = form.querySelector('.image-previewer')
                    if (imagePreviewer)
                        imagePreviewer.classList.remove('selected')
                })
            }
        })
    })

    // handle image previewer
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]')
        const imagePreview = previewer.querySelector('.image-preview')

        previewer.addEventListener('click', () => fileInput.click())

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0]
            if (file)
                processImage(file, imagePreview, previewer, previewSize)
        })
    })

    // handle submit forms
    const forms = document.querySelectorAll('form.ajax')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            clearErrorMessages(form)

            const formData = new FormData(form)
            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })

                if (res.ok) {
                    const modal = form.closest('.modal')
                    if (modal)
                        modal.style.display = 'none';

                    window.location.reload()
                }

                if (res.status === 400) {
                    const data = await res.json()

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (span) {
                                span.innerText = data.errors[key].join('\n')
                                span.classList.add('field-validation-error')
                            }
                        })
                    }
                }
            }
            catch {
                console.log('error submitting the form')
            }
        })
    })

    // hide form notification errors if they do not have an inner text
    const errorSpans = document.querySelectorAll('form .notification-error span');
    errorSpans.forEach(span => {
        if (!span.innerText.trim()) {
            span.closest('.notification-error').style.display = 'none';
        }
    })

    // load QuillJS
    // addProject
    const addProjectDescriptionTextarea = document.querySelector('.add-project-description-model-field')
    if (addProjectDescriptionTextarea) {
        const addProjectDescriptionQuill = new Quill('#add-project-description-wysiwyg-editor', {
            modules: {
                syntax: true,
                toolbar: '#add-project-description-wysiwyg-toolbar'
            },
            theme: 'snow',
            placeholder: 'Type something...'
        });

        addProjectDescriptionQuill.on('text-change', () => {
            addProjectDescriptionTextarea.value = addProjectDescriptionQuill.root.innerHTML
        });
    }

    // editProject
    const editProjectDescriptionTextarea = document.querySelector('.edit-project-description-model-field')
        if (editProjectDescriptionTextarea) {

        const editProjectDescriptionQuill = new Quill('#edit-project-description-wysiwyg-editor', {
            modules: {
                syntax: true,
                toolbar: '#edit-project-description-wysiwyg-toolbar'
            },
            theme: 'snow',
            placeholder: 'Type something...'
        });

        editProjectDescriptionQuill.on('text-change', () => {
            editProjectDescriptionTextarea.value = editProjectDescriptionQuill.root.innerHTML
        });
    }
})

function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
    })
}



async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader()

        reader.onerror = () => reject(new Error("Failed to load file."))
        reader.onload = (e) => {
            const img = new Image()
            img.onerror = () => reject(new Error("Failed to load image."))
            img.onload = () => resolve(img)
            img.src = e.target.result
        }

        reader.readAsDataURL(file)
    })
}


async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file)
        const canvas = document.createElement('canvas')
        canvas.width = previewSize
        canvas.height = previewSize

        const ctx = canvas.getContext('2d')
        ctx.drawImage(img, 0, 0, previewSize, previewSize)
        imagePreview.src = canvas.toDataURL('image/jpeg')
        previewer.classList.add('selected')
    }
    catch (error) {
        console.error('Failed on image processing: ', error)
    }
}

// Function to populate the edit client modal with data
function populateEditClientModal(client, modal) {
    const form = modal.querySelector('form');
    if (!form) return;

    // Set form values
    form.querySelector('input[name="Id"]').value = client.id;
    form.querySelector('input[name="IsActive"]').value = client.isActive;
    form.querySelector('input[name="Name"]').value = client.name;
    form.querySelector('input[name="Email"]').value = client.email;
    form.querySelector('input[name="PhoneNumber"]').value = client.phoneNumber || '';

    // Address fields
    if (client.clientAddress) {
        form.querySelector('input[name="ClientAddress.StreetAddress"]').value = client.clientAddress.streetAddress || '';
        form.querySelector('input[name="ClientAddress.PostalCode"]').value = client.clientAddress.postalCode || '';
        form.querySelector('input[name="ClientAddress.City"]').value = client.clientAddress.city || '';
        form.querySelector('input[name="ClientAddress.Country"]').value = client.clientAddress.country || '';
    }

    // If client has an image, display it
    if (client.imageURI) {
        const imagePreview = form.querySelector('.image-preview');
        if (imagePreview) {
            imagePreview.src = client.imageURI;
            form.querySelector('.image-previewer').classList.add('selected');
        }
    }
}

// Function to populate the edit member modal with data
function populateEditMemberModal(member, modal) {
    const form = modal.querySelector('form');
    if (!form) return;

    // Set form values
    form.querySelector('input[name="UserId"]').value = member.userId;
    form.querySelector('input[name="User.FirstName"]').value = member.user.firstName;
    form.querySelector('input[name="User.LastName"]').value = member.user.lastName;
    form.querySelector('input[name="User.Email"]').value = member.user.email;
    form.querySelector('input[name="PhoneNumber"]').value = member.phoneNumber || '';
    form.querySelector('input[name="JobTitle"]').value = member.jobTitle;

    // Address fields
    if (member.memberAddress) {
        form.querySelector('input[name="MemberAddress.StreetAddress"]').value = member.memberAddress.streetAddress || '';
        form.querySelector('input[name="MemberAddress.PostalCode"]').value = member.memberAddress.postalCode || '';
        form.querySelector('input[name="MemberAddress.City"]').value = member.memberAddress.city || '';
    }

    // If member has an image, display it
    if (member.imageURI) {
        const imagePreview = form.querySelector('.image-preview');
        if (imagePreview) {
            imagePreview.src = member.imageURI;
            form.querySelector('.image-previewer').classList.add('selected');
        }
    }
}

// Function to populate the edit project modal with data
function populateEditProjectModal(project, modal) {
    const form = modal.querySelector('form');
    if (!form) return;

    // Set form values
    form.querySelector('input[name="Id"]').value = project.id;
    form.querySelector('input[name="Name"]').value = project.name;
    form.querySelector('input[name="StartDate"]').value = project.startDate;
    form.querySelector('input[name="EndDate"]').value = project.endDate;
    form.querySelector('input[name="Budget"]').value = project.budget;
    form.querySelector('textarea[name="Description"]').value = project.description;
    const quillData = document.getElementById('edit-project-description-wysiwyg-editor').querySelector('.ql-editor');
    quillData.innerHTML = project.description;

    // If client has an image, display it
    if (project.imageURI) {
        const imagePreview = form.querySelector('.image-preview');
        if (imagePreview) {
            imagePreview.src = project.imageURI;
            form.querySelector('.image-previewer').classList.add('selected');
        }
    }
}