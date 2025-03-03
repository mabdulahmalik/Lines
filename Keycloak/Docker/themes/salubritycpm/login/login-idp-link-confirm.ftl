<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('confirmLinkIdpTitle')}">
            <form action="${url.loginAction}" method="post">
                <div class="${properties.btnWrapperClass!}">
                    <@components.button type="submit" name="submitAction" value="updateProfile">
                        ${msg("confirmLinkIdpReviewProfile")}
                    </@components.button>
                    <@components.button type="submit" name="submitAction" value="linkAccount">
                        ${msg("confirmLinkIdpContinue", idpDisplayName)}
                    </@components.button>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>
