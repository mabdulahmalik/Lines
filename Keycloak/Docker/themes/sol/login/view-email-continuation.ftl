<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=false; section>
    <#if section = "form">
        <@components.cardMain header="${msg('continueWithLinkInEmail')}" hasBackToLoginLink=true displayMessage=displayMessage>
            <@components.formField
                type="text"
                id="attempted-username"
                name="attempted-username"
                value="${auth.attemptedUsername}"
                booleanAttributes=["disabled"]
                labelText="${(!realm.loginWithEmailAllowed)?then(msg('username'), (!realm.registrationEmailAsUsername)?then(msg('usernameOrEmail'), msg('email')))}"
            />
            <div class="my-5">
                ${msg("magicLinkContinuationConfirmation")}
            </div>
        </@components.cardMain>
    </#if>
    <script>
        (function (w, d) {
            setTimeout(function(){
                w.location.reload(1);
            }, 5000);
        })(window, document);
    </script>
</@layout.registrationLayout>