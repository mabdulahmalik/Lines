<script setup lang="ts">
import { FwbButton, FwbInput } from 'flowbite-vue';
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import { BaseTable, THead, TR, TD, TH } from '@/components/table/index';
import draggable from 'vuedraggable';
import { ref, computed } from 'vue';
import { IconDotsReorder, IconArrowNarrowDown, IconPlusCircle } from '@/components/icons/index';
import { useForm, useFieldArray } from 'vee-validate';
import * as yup from 'yup';
import { ModifyFacilityRoomPropertyCmd } from '@/api/__generated__/graphql';
import { useFacilityTypesStore } from '@/stores/data/settings/facilityTypes';

const props = defineProps<{
  roomProperty: any;
  name: string;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
}>();

const facilityTypesStore = useFacilityTypesStore();

const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const roomPropertyOptions = ref(JSON.parse(JSON.stringify(props.roomProperty.options)));
const schemaRoomPropertyValues = yup.object({
  roomPropertyValues: yup.array().of(
    yup.object().shape({
      text: yup.string().required('This field is required').trim(),
      id: yup.string().nullable(),
    })
  ),
});
const initialRoomRopertyValues = roomPropertyOptions.value.map((item: any) => ({
  id: item.id,
  text: item.text,
}));

const { handleSubmit: handleSaveFTProperty, errors: roomPropertyValuesErrors } = useForm({
  initialValues: {
    roomPropertyValues: initialRoomRopertyValues,
  },
  validationSchema: schemaRoomPropertyValues,
});

const { fields: roomPropertyValues, push: pushToRoomPropertyValues,  swap: swapPropertyValue} =
  useFieldArray('roomPropertyValues');


const editingIndex = ref<number | null>(null);
const toggleEditIndex = (index: number) => {
  editingIndex.value = index;
};
function handleEditValue() {
  editingIndex.value = null;
}
const addInputField = () => {
  editingIndex.value = roomPropertyValues.value.length;
  pushToRoomPropertyValues({id: null, text: ''});
};

const submittedData = () => {
  console.log(roomPropertyValuesErrors, 'cr erros');
  handleSaveFTProperty(async (values) => {
    const updatedRoomProperty: ModifyFacilityRoomPropertyCmd = {
      facilityTypeId: props.roomProperty.facilityTypeId,
      id: props.roomProperty.id,
      name: props.name,
      options: values.roomPropertyValues
    }
    console.log(updatedRoomProperty, props.roomProperty);
    facilityTypesStore.modifyRoomProperty(updatedRoomProperty)
    closeRoomPropertyDrawer();
  })();
};

// drag & drop
const dragOptions = computed(() => {
  return {
    animation: 200,
    group: 'table-items',
    disabled: false,
    ghostClass: 'shadow',
  };
});

defineExpose({
  submittedData,
});

const closeRoomPropertyDrawer = () => {
  emit('close');
};

const sortRoomPropertyValue = (e: any) => {
  const { oldIndex, newIndex } = e;
  swapPropertyValue(oldIndex, newIndex);
};

</script>

<template>
  <div class="flex h-auto lg:h-full flex-col lg:flex-row">
    <!-- Left panel -->
    <div
      class="h-auto min-h-full lg:h-full w-full lg:w-80 lg:border-r border-slate-300 overflow-y-auto custom-scroll py-4"
    >
      <div class="px-4">
        <!-- Required procedures fields -->
        <AccordionDefault id="1" active class="pb-2 lg:pb-4 border-slate-300">
          <template #header>
            <div class="w-full flex items-center justify-between">
              <div>Associated with</div>
            </div>
          </template>

          <!-- Display facility name -->
          <div class="flex justify-between py-3">
            <div class="text-xs font-medium text-slate-500">Facility Type</div>
            <div
              class="rounded-full bg-[#DEF7EC] py-[2px] px-[10px] text-[#057A55] text-xs font-medium"
            >
              {{ props.roomProperty.facilityType }}
            </div>
          </div>
        </AccordionDefault>
      </div>
      <hr class="border-slate-300" />
    </div>

    <!-- Right panel -->
    <div class="h-full w-full lg:flex-1 py-4 lg:px-10 px-4 overflow-y-auto custom-scroll">
      <div class="text-xl font-semibold">Values</div>

      <!-- Table start -->
      <div class="my-8">
        <base-table>
          <t-head class="uppercase">
            <t-r>
              <t-h class="w-6"></t-h>
              <t-h class="w-16">
                <div class="text-slate-800 flex items-center gap-2">
                  <span>Order</span>
                  <IconArrowNarrowDown />
                </div>
              </t-h>
              <t-h>Name</t-h>
            </t-r>
          </t-head>
          <draggable
            v-model="roomPropertyValues"
            item-key="id"
            tag="tbody"
            v-bind="dragOptions"
            handle=".draggable-handle-icon"
            @end="sortRoomPropertyValue"
          >
            <template #item="{ element, index }">
              <t-r class="group hover:bg-slate-50">
                <t-d class="w-6">
                  <IconDotsReorder
                    class="cursor-grab active:cursor-grabbing draggable-handle-icon"
                  />
                </t-d>
                <t-d>{{ index + 1 }}</t-d>
                <t-d class="text-brand-600 font-semibold py-1" @click="toggleEditIndex(index)">
                  <span v-if="editingIndex !== index">
                    <span v-if="roomPropertyValuesErrors[`roomPropertyValues[${index}].text` as any]" class="text-sm text-radical-red-600">
                      {{ roomPropertyValuesErrors[`roomPropertyValues[${index}].text` as any] }}
                    </span>
                    {{ element.value.text }}
                  </span>
                  <div v-else class="flex gap-2">
                    <fwb-input v-model="element.value.text" class="w-full" size="sm" 
                    
                    :class="
                    roomPropertyValuesErrors[`roomPropertyValues[${index}].text` as any]
                      ? inputErrorClasses
                      : ''
                  "
                  />
                    <fwb-button
                      @click.stop="handleEditValue"
                      class="bg-primary-600 hover:bg-brand-600"
                      pill
                      size="sm"
                      >Confirm</fwb-button
                    >
                  </div>
                </t-d>
              </t-r>
            </template>
          </draggable>
        </base-table>
        <div class="mt-4">
          <button
            @click="addInputField"
            class="flex items-center text-brand-600 text-sm font-medium"
          >
            <IconPlusCircle class="mr-0.5" /> Add Value
          </button>
        </div>
      </div>
      <!-- Table end -->
    </div>
  </div>
</template>
