<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>

<@layout.registrationLayout; section>
    <#if section = "form">
        <@components.cardMain header="${msg('loginChooseAuthenticator')}">
            <form id="kc-select-credential-form" action="${url.loginAction}" method="post">
                <div class="space-y-10 my-10">
                    <#list auth.authenticationSelections as authenticationSelection>
                        <div>
                            <@components.button variant="primary" class="${properties.kcSelectAuthListItemClass!}" type="submit" name="authenticationExecution" value="${authenticationSelection.authExecId}">
                                ${msg('${authenticationSelection.displayName}')}
                            </@components.button>
                            <div class="text-center">
                                ${msg('${authenticationSelection.helpText}')}
                            </div>
                        </div>
                    </#list>
                </div>
            </form>
        </@components.cardMain>
    </#if>
</@layout.registrationLayout>

