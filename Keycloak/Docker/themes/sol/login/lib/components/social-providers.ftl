<#import "../components.ftl" as components>

<#macro socialProviders>
    <#if realm.password && social?? && social.providers?has_content>
        <div class="grid grid-cols-1 py-5 gap-3">
            <#list social.providers as p>
                <@components.link href="${p.loginUrl}" style="text-decoration:none">
                    <button class="text-black font-medium w-full border rounded-full flex space-x-3 py-2 justify-center items-center">
                        <img src="${url.resourcesPath}/img/social-${p.alias}.svg" alt="${p.alias}-icon" width="20rem" height="20rem">
                        <span class="block">
                            ${p.displayName!}
                        </span>
                    </button>
                </@components.link>
            </#list>
        </div>
    </#if>
</#macro>