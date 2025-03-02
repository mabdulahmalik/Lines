<script setup lang="ts">
import { FwbButton, FwbInput } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ref } from 'vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';

// Props
const props = defineProps<{ fName: string; lName: string }>();

// Emits
const emit = defineEmits(['name-updated']);

// Valitiation
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaName = yup.object({
  fName: yup.string().required('This is a required field'),
  lName: yup.string().required('This is a required field'),
});
const { handleSubmit } = useForm({
  validationSchema: schemaName,
  initialValues: {
    fName: props.fName,
    lName: props.lName,
  },
});

// First & Last Name
const { value: fName, errorMessage: fNameError } = useField<string>('fName');
const { value: lName, errorMessage: lNameError } = useField<string>('lName');

// Save Name edit
const handleSave = () => {
  handleSubmit(async () => {
    emit('name-updated', { fName: fName.value, lName: lName.value });
    closeModal();
  })();
};

// Modal
const editUserNameModalRef = ref<InstanceType<typeof Modal>>();
function closeModal() {
  editUserNameModalRef.value?.setModalOpen(false);
}
const setModalOpen = (value: boolean): void => {
  editUserNameModalRef.value?.setModalOpen(value);
};

// Expose function
defineExpose({
  setModalOpen,
});
</script>

<template>
  <Modal title="Edit User Name" :z_index="61" ref="editUserNameModalRef">
    <template #body>
      <div class="flex flex-col p-4 lg:p-6 gap-4">
        <div>
          <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
            >First name</label
          >
          <fwb-input
            v-model="fName"
            type="text"
            placeholder="Write text here"
            :class="fNameError ? inputErrorClasses : ''"
          />
          <span v-if="fNameError" class="text-sm text-radical-red-600">{{ fNameError }}</span>
        </div>
        <div>
          <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
            >Last name</label
          >
          <fwb-input
            v-model="lName"
            type="text"
            placeholder="Select a date"
            :class="lNameError ? inputErrorClasses : ''"
          />
          <span v-if="lNameError" class="text-sm text-radical-red-600">{{ lNameError }}</span>
        </div>
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="handleSave" class="bg-primary-600 hover:bg-brand-600" pill>
          Confirm
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
