function googleRecaptcha(dotNetObject, selector, sitekeyvalue) { 
    return grecaptcha.render(selector, { 
        'sitekey': '6Lcu4zIqAAAAAGwRGrbhwIT_VWW4NVA5tbnSKgDB',
        'callback': (response) => { dotNetObject.invokeMethodAsync('CallbackOnSuccess', response); },
        'expired-callback': () => { dotNetObject.invokeMethodAsync('CallbackOnExpired', response); }
    });
};


function getResponse(response) {
    return grecaptcha.getResponse(response);
}