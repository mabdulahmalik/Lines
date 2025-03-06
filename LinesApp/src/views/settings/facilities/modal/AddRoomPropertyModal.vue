<script setup lang="ts">
import { ref, onUnmounted, watch } from 'vue';
import { FwbInput, FwbButton } from 'flowbite-vue';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';
import { IconPlusCircle } from '@/components/icons/index';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { useModalStore } from '@/stores/modal';
import { useForm, useField, useFieldArray } from 'vee-validate';
import * as yup from 'yup';
import { FacilityRoomProperty } from '@/api/__generated__/graphql';

interface RoomInputField {
  value: {
    text: string;
  };
}

// Emits
const emit = defineEmits<{
  (e: 'width', val: string): void;
  (e: 'property-added', val: FacilityRoomProperty): void;
}>();

const facilityTypesStore = useFacilityTypesStore();
const modalStore = useModalStore();

// Room Properties / Facility Type Properties
const modalAddRoomRef = ref<InstanceType<typeof ModalDrawer>>();
const facilityTypeProperties = ref([...facilityTypesStore.roomProperties]);

watch(
  () => facilityTypesStore.roomProperties,
  (newValue) => {
    facilityTypeProperties.value = [...newValue];
  },
  { deep: true }
);

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaFacilityRoomProperty = yup.object({
  name: yup.string().required('This is a required field').trim(),
  options: yup.array().of(
    yup.object().shape({
      text: yup.string().required('This field is required').trim(),
      id: yup.string().nullable(),
    })
  ),
});
const initialoptions = [{ text: '', id: null }];
const { handleSubmit: handleSaveFRProperty, errors: createRoomErrors } = useForm({
  initialValues: {
    name: '',
    options: initialoptions,
  },
  validationSchema: schemaFacilityRoomProperty,
});
const { value: name, errorMessage: nameError, resetField: resetname } = useField<string>('name');
const { fields: options, push: pushTooptions, replace: replaceoptions } = useFieldArray('options');

const addInputField = () => {
  pushTooptions({ text: '', id: null });
};
const isAddFRPropertySubmit = ref(false);
function handleAddFRProperty() {
  isAddFRPropertySubmit.value = true;
  handleSaveFRProperty(async (values) => {
    const newFRProperty: FacilityRoomProperty = {
      id: null,
      name: name.value,
      options: options.value.map((item) => ({
        text: (item.value as RoomInputField['value']).text,
        id: null,
      })),
    };
    // facilityTypesStore.addRoomProperty(newFRProperty);
    console.log(newFRProperty, 'vales', values);
    emit('property-added', newFRProperty);
    isAddFRPropertySubmit.value = false;
    resetname();
    replaceoptions([{ text: '', id: null }]);
    modalAddRoomRef.value?.setModalOpen(false);
  })();
}

function handleCancelAddRoom() {
  modalAddRoomRef.value?.setModalOpen(false);
}

const setModalOpen = (value: boolean): void => {
  modalAddRoomRef.value?.setModalOpen(value);
};
function handleModalClosed() {
  if (modalStore.isFacilityTypeModalAutoExpended) {
    emit('width', '5xl');
    modalStore.isFacilityTypeModalExpended = false;
    modalStore.isFacilityTypeModalAutoExpended = false;
  }
}
defineExpose({
  setModalOpen,
});
onUnmounted(() => {
  modalStore.isModalDrawerExpended = false;
});
</script>
<template>
  <ModalDrawer
    ref="modalAddRoomRef"
    close_outside
    max_width="xl"
    title="Add Room Property"
    @close="handleModalClosed"
  >
    <template #body>
      <!-- body -->
      <div class="flex flex-col px-4 py-2 lg:px-8 lg:py-8">
        <div>
          <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
            >Property Name</label
          >
          <fwb-input
            v-model="name"
            placeholder="Write text here"
            :class="nameError ? inputErrorClasses : ''"
          />
          <span v-if="nameError" class="text-sm text-radical-red-600">{{ nameError }}</span>
        </div>

        <hr class="mt-6 text-slate-300" />
        <div class="my-6">
          <div class="text-xl font-semibold">Add Values</div>
          <!-- input box -->
          <div v-for="(field, index) in options" :key="field.key" class="my-4">
            <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
              >Value {{ index + 1 }}</label
            >
            <fwb-input
              v-model="(field.value as RoomInputField['value']).text"
              placeholder="Write text here"
              :class="
                createRoomErrors[`options[${index}].text` as keyof typeof createRoomErrors] &&
                isAddFRPropertySubmit
                  ? inputErrorClasses
                  : ''
              "
            />
            <span
              v-if="
                createRoomErrors[`options[${index}].text` as keyof typeof createRoomErrors] &&
                isAddFRPropertySubmit
              "
              class="text-sm text-radical-red-600"
              >{{
                createRoomErrors[`options[${index}].text` as keyof typeof createRoomErrors]
              }}</span
            >
          </div>
        </div>
        <hr class="mb-4 -mt-4 text-slate-300" />
        <button @click="addInputField" class="flex items-center text-brand-600 text-sm font-medium">
          <IconPlusCircle class="mr-0.5" /> Add Value
        </button>
      </div>
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex items-center justify-end w-full">
        <div class="flex items-center gap-4 lg:gap-6">
          <span
            v-if="Object.keys(createRoomErrors).length && isAddFRPropertySubmit"
            class="text-sm font-normal text-radical-red-600"
            >{{ Object.keys(createRoomErrors).length }} errors</span
          >
          <fwb-button @click="handleCancelAddRoom" color="light" pill> Cancel</fwb-button>
          <fwb-button
            @click.prevent="handleAddFRProperty"
            class="bg-primary-600 hover:bg-brand-600 w-full lg:w-auto"
            pill
          >
            Save
          </fwb-button>
        </div>
      </div>
    </template>
  </ModalDrawer>
</template>
