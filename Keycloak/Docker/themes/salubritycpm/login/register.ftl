<#import "template.ftl" as layout>
<#import "lib/components.ftl" as components>
<#import "lib/commons.ftl" as commons>

<@layout.registrationLayout displayMessage=messagesPerField.exists('global') imgRight="create-account"; section>
    <#if section = "form">
		<@components.cardMain header="${msg('registerTitle')}">
			<form action="${url.registrationAction}" method="post">

				<@commons.userProfileFormFields; callback, attribute>
					<#if callback = "afterField">
					<#-- render password fields just under the username or email (if used as username) -->
						<#if passwordRequired?? && (attribute.name == 'username' || (attribute.name == 'email' && realm.registrationEmailAsUsername))>
							<@components.formField
								type="password"
								id="password"
								name="password"
								autocomplete="new-password"
								booleanAttributes=["required"]
								labelText="${msg('password')}"
								hasError=messagesPerField.existsError('password')
								errorMessage="${kcSanitize(messagesPerField.get('password'))?no_esc}"
							/>

							<@components.formField
								type="password"
								id="password-confirm"
								name="password-confirm"
								autocomplete="new-password"
								booleanAttributes=["required"]
								labelText="${msg('passwordConfirm')}"
								hasError=messagesPerField.existsError('password-confirm')
								errorMessage="${kcSanitize(messagesPerField.get('password-confirm'))?no_esc}"
							/>
						</#if>
					</#if>
				</@commons.userProfileFormFields>

				<#if termsAcceptanceRequired??>
					<@components.terms/>
					<@components.formField
						type="checkbox"
						id="termsAccepted"
						name="termsAccepted"
						booleanAttributes=["required", "${(login.termsAccepted)???then('checked', '')}"]
						labelText="${msg('acceptTerms')}"
						hasError=messagesPerField.existsError('termsAccepted')
						errorMessage="${kcSanitize(messagesPerField.get('termsAccepted'))?no_esc}"
					/>
				</#if>


				<@components.recaptcha/>

				<div class="${properties.btnWrapperClass!}">
					<@components.button type="submit" variant="primary">
						${msg("doRegister")}
					</@components.button>
				</div>
			</form>

			<@components.divider/>

			<@components.socialProviders/>

			<hr/>
<#--			<div class="${properties.textCenterClass!}">-->
			<div class="">
				${kcSanitize(msg("alreadyHaveAnAccount"))?no_esc}
				<@components.link href="${url.loginUrl}">
					${kcSanitize(msg("signin"))?no_esc}
				</@components.link>
			</div>
		</@components.cardMain>
    </#if>
</@layout.registrationLayout>
