$("#js-login-btn").click(function (event) {

    // Fetch form to apply custom Bootstrap validation
    var form = $("#js-login")

    if (form[0].checkValidity() === false) {
        event.preventDefault()
        event.stopPropagation()
    }

    form.addClass('was-validated');
    // Perform ajax submit here...
});

