<script setup lang="ts">
import { FwbButton, FwbInput } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ref } from 'vue';
import { Line } from '@/api/__generated__/graphql';
import { useLinesStore } from '@/stores/data/encounters/lines';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';

const props = defineProps<{
  line: Line;
}>();

const linesStore = useLinesStore();

const markInfectionModalRef = ref<InstanceType<typeof Modal>>();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaInfectionDate = yup.object({
  infectedOn: yup.string().required('This is a required field'),
});
const { handleSubmit } = useForm({
  validationSchema: schemaInfectionDate,
});
const { value: infectedOn, errorMessage: infectedOnError } = useField<string>('infectedOn');

function markInfection() {
  handleSubmit(async (values) => {
    if (props.line.id) {
      linesStore.recordLineInfection({ id: props.line.id, infectedOn: values.infectedOn });
      markInfectionModalRef.value?.setModalOpen(false);
    }
    markInfectionModalRef.value?.setModalOpen(false);
  })();
}
const setModalOpen = (value: boolean): void => {
  markInfectionModalRef.value?.setModalOpen(value);
};
function closeModal() {
  markInfectionModalRef.value?.setModalOpen(false);
}

defineExpose({
  setModalOpen,
});
</script>

<template>
  <Modal title="Mark Has Infection" :z_index="61" ref="markInfectionModalRef">
    <template #body>
      <div class="p-4 lg:p-6">
        <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
          >Infected on</label
        >
        <fwb-input
          v-model="infectedOn"
          type="date"
          placeholder="Select a date"
          :class="infectedOnError ? inputErrorClasses : ''"
        />
        <span v-if="infectedOnError" class="text-sm text-radical-red-600">{{
          infectedOnError
        }}</span>
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="markInfection" class="bg-primary-600 hover:bg-brand-600" pill>
          Confirm
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
