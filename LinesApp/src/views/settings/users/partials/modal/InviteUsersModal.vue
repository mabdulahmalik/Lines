<script setup lang="ts">
import { computed, ref } from 'vue';
import { FwbButton, FwbTextarea, FwbCheckbox } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { IconPlusCircle } from '@/components/icons';
import { useForm, useFieldArray } from 'vee-validate';
import * as yup from 'yup';
import { useRolesStore } from '@/stores/data/settings/users/roles';
import { useInvitationsStore } from '@/stores/data/settings/users/invitations';

// Store instances
const rolesStore = useRolesStore();
const invitationsStore = useInvitationsStore();

// Computed available roles (excluding "Owner")
const availableRoles = computed(() => rolesStore.roles.filter((role) => role.name !== 'Owner'));

// Define Group Type
interface Group {
  emails: string[];
  roles: string[];
}

// Validation Schema
const schema = yup.object({
  groups: yup.array().of(
    yup.object({
      emails: yup
        .array()
        .of(
          yup
            .string()
            .matches(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/, 'Invalid email format')
        )
        .min(1, 'At least one email is required')
        .max(10, 'You can enter up to 10 email addresses only'),
      roles: yup.array().min(1, 'At least one role must be selected'),
    })
  ),
});

// Initialize form with validation
const { handleSubmit, values, errors, resetForm } = useForm<{ groups: Group[] }>({
  validationSchema: schema,
  initialValues: {
    groups: [{ emails: [], roles: [] }],
  },
  validateOnMount: false,
});

// Manage groups with `useFieldArray`
const { fields: groups, push, update } = useFieldArray<Group>('groups');

// Store raw user input for emails (so users can type freely)
const rawEmails = ref<string[]>(values.groups.map(() => ''));

// Add a new group
const addNewGroup = () => {
  push({ emails: [], roles: [] });
  rawEmails.value.push('');
};

// Handle raw input for emails (so user can type freely)
const handleEmailsInput = (index: number, event: Event) => {
  rawEmails.value[index] = (event.target as HTMLTextAreaElement).value;
};

// Process emails when user blurs the field
const processEmails = (index: number) => {
  const emailArray = rawEmails.value[index]
    .split(/[\s,]+/) // Split by spaces or commas
    .map((email) => email.trim()) // Trim whitespace
    .filter((email) => email.length > 0); // Remove empty strings
  const uniqueEmails = Array.from(new Set(emailArray));
  update(index, { ...values.groups[index], emails: uniqueEmails });
  rawEmails.value[index] = uniqueEmails.join(', ');
};

// Define a method to get email errors for a specific group
const getEmailErrorsForGroup = (groupIndex: number): string[] => {
  const emailErrors: string[] = [];
  Object.entries(errors.value).forEach(([key, value]) => {
    const match = key.match(/groups\[(\d+)\]\.emails(?:\[(\d+)\])?/);
    if (match && parseInt(match[1], 10) === groupIndex) {
      emailErrors.push(value as string);
    }
  });
  return emailErrors;
};

// Handle role selection
const toggleRoleSelection = (index: number, roleId: string) => {
  const group = values.groups[index];
  const updatedRoles = group.roles.includes(roleId)
    ? group.roles.filter((id) => id !== roleId)
    : [...group.roles, roleId];

  update(index, { ...group, roles: updatedRoles });
};

const showErrors = ref(false);

// Submit invitations
const handleSendInvites = () => {
  showErrors.value = true;
  handleSubmit(async (values) => {
    for (const group of values.groups) {
      invitationsStore.inviteUsers({ emails: group.emails, roles: group.roles });
      console.log(group, 'group');
    }
    closeInviteUsersModal();
  })();
};

// Modal controls
const modalInviteUsers = ref<InstanceType<typeof ModalDrawer>>();
const closeInviteUsersModal = () => {
  resetForm();
  modalInviteUsers.value?.setModalOpen(false);
};
const setModalOpen = (value: boolean) => modalInviteUsers.value?.setModalOpen(value);
defineExpose({ setModalOpen });
</script>

<template>
  <ModalDrawer ref="modalInviteUsers" close_outside max_width="xl" title="Invite Users" @close="showErrors =false">
    <template #body>
      <div class="flex flex-col px-4 py-2 lg:px-8 lg:py-8">
        <div v-for="(_, index) in groups" :key="index" class="mb-8">
          <div class="text-xl leading-[30px] font-semibold text-slate-900 mb-4">
            Group {{ index + 1 }}
          </div>

          <label class="mb-1 block text-sm font-medium text-slate-900"
            >Enter up to 10 email addresses, separated by comma or space</label
          >
          <!-- Email Input -->
          <fwb-textarea
            v-model="rawEmails[index]"
            @input="handleEmailsInput(index, $event)"
            @blur="processEmails(index)"
            :rows="5"
            placeholder="name@domain.com, email@domain.com"
            label=""
            :class="{ 'border-red-500': showErrors && getEmailErrorsForGroup(index).length > 0 }"
          />
          <p v-if="showErrors && getEmailErrorsForGroup(index).length > 0" class="text-red-500 text-sm">
            {{ getEmailErrorsForGroup(index)[0] }}
          </p>
          <!-- Role Selection -->
          <div>
            <div class="text-sm font-medium my-4 text-slate-900">Assign roles</div>
            <div class="flex gap-5 flex-wrap mb-1">
              <fwb-checkbox
                v-for="role in availableRoles"
                :key="role.id"
                :label="role.name"
                :checked="values.groups[index].roles.includes(role.id)"
                @change="toggleRoleSelection(index, role.id)"
              />
            </div>
            <p v-if="showErrors && errors[(`groups[${index}].roles`) as any]" class="text-red-500 text-sm">
              {{ errors[`groups[${index}].roles` as any] }}
            </p>
          </div>
          <hr class="mt-6 text-slate-300" />
        </div>
        <!-- Add new group button -->
        <button
          @click="addNewGroup"
          class="flex -mt-4 items-center gap-2 text-brand-600 text-sm font-medium"
        >
          <IconPlusCircle /> Add new group
        </button>
      </div>
    </template>

    <template #footer>
      <div class="p-4 lg:px-6 flex items-center justify-end w-full">
        <div class="flex gap-6">
          <fwb-button @click="closeInviteUsersModal" color="light" pill>Cancel</fwb-button>
          <fwb-button
            @click="handleSendInvites"
            class="bg-primary-600 hover:bg-brand-600 w-full lg:w-auto"
            pill
          >
            Send Invites
          </fwb-button>
        </div>
      </div>
    </template>
  </ModalDrawer>
</template>
