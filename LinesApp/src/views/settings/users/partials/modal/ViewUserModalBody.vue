<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { FwbInput, FwbButton, FwbCheckbox } from 'flowbite-vue';
import { ModifyUserCmd, User } from '@/api/__generated__/graphql';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import AccordionHeader from '@/views/encounters/jobs/partials/AccordionHeader.vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { useRolesStore } from '@/stores/data/settings/users/roles';
import { useUsersStore } from '@/stores/data/settings/users';
import { formatRelativeDate } from '@/utils/dateUtils';

const props = defineProps<{
  user: User;
  fName: string;
  lName: string;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
}>();

const rolesStore = useRolesStore();
const usersStore = useUsersStore();

const email = ref('');
const phone = ref('');

onMounted(() => {
  email.value = props.user.email || '';
  phone.value = props.user.phone || '';
});
watch(
  () => props.user,
  (usr) => {
    email.value = usr.email || '';
    phone.value = usr.phone || '';
  }
);

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');

const schemaInfo = yup.object({
  emailEdit: yup.string().required('This is a required field'),
  phoneEdit: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmitInfo } = useForm({
  validationSchema: schemaInfo,
});
const { value: emailEdit, errorMessage: emailEditError } = useField<string>('emailEdit');
const { value: phoneEdit, errorMessage: phoneEditError } = useField<string>('phoneEdit');
const isEditInformation = ref(false);

function handleEditIconInformation() {
  emailEdit.value = email.value;
  phoneEdit.value = phone.value;
  isEditInformation.value = true;
}

function handleConfirmEdit() {
  handleSubmitInfo(async () => {
    email.value = emailEdit.value;
    phone.value = phoneEdit.value;
    isEditInformation.value = false;
    console.log('Edit user info', email.value, phone.value);
  })();
}

// Roles
const adminRole = rolesStore.roles.find((role) => role.name === 'Adiministrator');
const directorRole = rolesStore.roles.find((role) => role.name === 'Director');
const clinicianRole = rolesStore.roles.find((role) => role.name === 'Clinician');

const schemaRoles = yup.object({
  selectedRoles: yup
    .array()
    .min(1, 'At least one role must be selected')
    .required('At least one role must be selected'),
});

const { handleSubmit: handleSubmitRoles } = useForm({
  validationSchema: schemaRoles,
});

const { value: selectedRoles, errorMessage: selectedRolesError } =
  useField<string[]>('selectedRoles');

const isAdmin = ref(false);
const isDirector = ref(false);
const isClinician = ref(false);

watch(
  () => props.user.roles,
  (roles) => {
    isAdmin.value = roles?.find((role) => role?.id === adminRole?.id) ? true : false;
    isDirector.value = roles?.find((role) => role?.id === directorRole?.id) ? true : false;
    isClinician.value = roles?.find((role) => role?.id === clinicianRole?.id) ? true : false;
    selectedRoles.value = roles?.map((role) => role?.id) as string[];
  },
  { immediate: true }
);

const selectedRolesComputed = computed(() => {
  return [
    isAdmin.value ? adminRole?.id : null,
    isDirector.value ? directorRole?.id : null,
    isClinician.value ? clinicianRole?.id : null,
  ].filter((role) => role !== null) as string[];
});

// Sync computed property with validation
watch(selectedRolesComputed, (newRoles) => {
  selectedRoles.value = newRoles;
});

const inTraning = ref(props.user.inTraining || false);

const modifyUser = () => {
  handleSubmitRoles(async () => {
    const updatedUser: ModifyUserCmd = {
      id: props.user.id,
      firstName: props.fName,
      lastName: props.lName,
      email: email.value,
      phone: phone.value,
      roles: [
        isAdmin.value ? adminRole?.id : null,
        isDirector.value ? directorRole?.id : null,
        isClinician.value ? clinicianRole?.id : null,
      ].filter((role) => role !== null) as string[],
      inTraining: inTraning.value,
    };
    usersStore.modifyUser(updatedUser);
  })();
};

defineExpose({
  modifyUser,
});
</script>

<template>
  <div class="flex h-auto lg:h-full flex-col lg:flex-row">
    <!-- Left panel -->
    <div
      class="h-auto min-h-full lg:h-full w-full lg:w-80 lg:border-r border-slate-300 overflow-y-auto custom-scroll py-4"
    >
      <div class="px-4">
        <AccordionDefault id="1" active class="pb-2 lg:pb-4 border-slate-300">
          <template #header>
            <AccordionHeader
              v-if="true"
              title="Information"
              @edit="handleEditIconInformation"
              :is-edit="isEditInformation"
            ></AccordionHeader>
            <span v-else class="text-base font-semibold">Location</span>
          </template>
          <div v-if="!isEditInformation" class="flex flex-col gap-4 pt-2">
            <div class="flex items-center gap-1">
              <label class="w-1/5 text-xs font-medium text-slate-500">Email</label>
              <div class="text-sm font-semibold text-slate-900">
                {{ email }}
              </div>
            </div>
            <div class="flex items-center gap-1">
              <label class="w-1/5 text-xs font-medium text-slate-500">Phone</label>
              <div class="text-sm font-semibold text-slate-900">
                {{ phone }}
              </div>
            </div>
          </div>
          <div v-else class="flex flex-col gap-4 pt-2">
            <div>
              <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                >Email</label
              >
              <fwb-input
                v-model="emailEdit"
                placeholder="Write text here"
                :class="emailEditError ? inputErrorClasses : ''"
              />
              <span v-if="emailEditError" class="text-sm text-radical-red-600">{{
                emailEditError
              }}</span>
            </div>
            <div>
              <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
                >Phone</label
              >
              <fwb-input
                v-model="phoneEdit"
                placeholder="Write text here"
                :class="phoneEditError ? inputErrorClasses : ''"
              />
              <span v-if="phoneEditError" class="text-sm text-radical-red-600">{{
                phoneEditError
              }}</span>
            </div>
            <div class="flex gap-4 items-center justify-end">
              <fwb-button @click="isEditInformation = false" color="light" pill>
                Cancel
              </fwb-button>
              <fwb-button @click="handleConfirmEdit" class="bg-primary-600 hover:bg-brand-600" pill>
                Confirm
              </fwb-button>
            </div>
          </div>
        </AccordionDefault>
      </div>
      <hr class="border-slate-300" />
      <div v-if="props.user?.registeredAt" class="p-4 flex flex-col gap-2">
        <div class="text-xs font-medium text-slate-500">
          Created:
          {{ formatRelativeDate(props.user.registeredAt) }}
        </div>
      </div>
    </div>

    <!-- Right panel -->
    <div class="h-full w-full lg:flex-1 py-4 lg:px-10 px-4 overflow-y-auto custom-scroll">
      <div class="text-xl font-semibold">User settings</div>
      <div>
        <div class="pt-8">
          <div class="text-sm font-semibold">Roles</div>
          <div class="flex flex-col gap-3 pt-4">
            <fwb-checkbox v-model="isAdmin" label="Administrator"></fwb-checkbox>
            <fwb-checkbox v-model="isDirector" label="Director"></fwb-checkbox>
            <fwb-checkbox v-model="isClinician" label="Clinician"></fwb-checkbox>
            <span v-if="selectedRolesError" class="text-sm text-radical-red-600">{{
              selectedRolesError
            }}</span>
          </div>
        </div>
        <div class="pt-8">
          <div class="text-sm font-semibold">Traning</div>
          <div class="flex flex-col gap-3 pt-4">
            <fwb-checkbox v-model="inTraning" label="Enrolled"></fwb-checkbox>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
