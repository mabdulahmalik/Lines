<script setup lang="ts">
import { FwbButton, FwbInput } from 'flowbite-vue';
import Modal from '@/components/modal/Modal.vue';
import { ref } from 'vue';
import { Line, PlaceLineExternallyCmd } from '@/api/__generated__/graphql';
import { useLinesStore } from '@/stores/data/encounters/lines';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';

const props = defineProps<{
  line: Line;
}>();

const linesStore = useLinesStore();

const markExternalPlacementModalRef = ref<InstanceType<typeof Modal>>();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaInfectionDate = yup.object({
  placedOn: yup.string().required('This is a required field'),
});
const { handleSubmit } = useForm({
  validationSchema: schemaInfectionDate,
});
const { value: placedOn, errorMessage: placedOnError } = useField<string>('placedOn');
const placedBy = ref('');
function markExternalPlacement() {
  handleSubmit(async (values) => {
    if (props.line.id) {
      const externalPlacement: PlaceLineExternallyCmd = {
        id: props.line.id,
        placedBy: placedBy.value,
        placedOn: values.placedOn,
      };
      linesStore.placeLineExternally(externalPlacement);
      markExternalPlacementModalRef.value?.setModalOpen(false);
    }
    markExternalPlacementModalRef.value?.setModalOpen(false);
  })();
}
const setModalOpen = (value: boolean): void => {
  markExternalPlacementModalRef.value?.setModalOpen(value);
};
function closeModal() {
  markExternalPlacementModalRef.value?.setModalOpen(false);
}

defineExpose({
  setModalOpen,
});
</script>

<template>
  <Modal title="Place Line Externally" :z_index="61" ref="markExternalPlacementModalRef">
    <template #body>
      <div class="flex flex-col p-4 lg:p-6 gap-4">
        <div>
          <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
            >Placed by</label
          >
          <fwb-input v-model="placedBy" type="text" placeholder="Write text here" />
        </div>
        <div>
          <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
            >Placed on</label
          >
          <fwb-input
            v-model="placedOn"
            type="date"
            placeholder="Select a date"
            :class="placedOnError ? inputErrorClasses : ''"
          />
          <span v-if="placedOnError" class="text-sm text-radical-red-600">{{ placedOnError }}</span>
        </div>
      </div>
    </template>
    <template #footer>
      <div class="py-4 px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeModal" color="light" pill> Cancel</fwb-button>
        <fwb-button @click="markExternalPlacement" class="bg-primary-600 hover:bg-brand-600" pill>
          Confirm
        </fwb-button>
      </div>
    </template>
  </Modal>
</template>
