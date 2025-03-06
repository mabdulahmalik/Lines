<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout displayMessage=false; section>
    <#if section = "form">
        <@components.cardMain header="${msg('invitations')}" displayMessage=displayMessage>
            <div class="my-5">
                You have been invited to join the following organizations. Uncheck those you wish to decline.
            </div>
            <form class="form-actions" action="${url.loginAction}" method="POST">
                <#list invitations.orgs as org>
                    <label>
                        <div class="py-2 px-4 border rounded-md w-full">
                            <input id="org-${org.id}" name="orgs" type="checkbox" value="${org.id}" checked> ${org.displayName}
                        </div>
                    </label>
                </#list>
                <@components.button variant="primary" name="accept" id="kc-accept" type="submit">
                    ${msg("acceptAndContinue")}
                </@components.button>
            </form>
            <div class="clearfix"></div>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>