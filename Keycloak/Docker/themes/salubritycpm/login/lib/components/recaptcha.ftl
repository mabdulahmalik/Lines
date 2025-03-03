<#macro recaptcha>
    <#if recaptchaRequired??>
        <div class="g-recaptcha" data-size="compact" data-sitekey="${recaptchaSiteKey}"></div>
    </#if>
</#macro>