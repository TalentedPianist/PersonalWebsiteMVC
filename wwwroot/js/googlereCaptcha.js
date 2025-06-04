const sitekeyValue = "6LeCBlUrAAAAAGJFT1Rt-4hojR6NfEvqzsvZwnOz";

function googleRecaptcha(dotNetObject, selector) { 
    return grecaptcha.render(selector, { 
        'sitekey': sitekeyValue, 
        'callback': (response) => { dotNetObject.invokeMethodAsync('CallbackOnSuccess', response); },
        'expired-callback': () => { dotNetObject.invokeVoidAsync('CallbackOnExpired', response); } 
    });
};

function getResponse(response) { 
    return grecaptcha.getResponse(response);
}
