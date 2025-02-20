<script setup lang="ts">
import { IconPlusCircle, IconMinusCircle } from '@/components/icons/index';
import { FwbSelect, FwbInput, FwbToggle } from 'flowbite-vue';
import { reactive, computed, watch, onMounted } from 'vue';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import Multiselect from 'vue-multiselect';
import { ProcedureFieldSetting, ProcedureFieldType, Routine } from '@/api/__generated__/graphql';

const props = defineProps<{
  routine?: Routine;
  source: string;
}>();
const emit = defineEmits(['trigger-updated', 'complete-setp']);

const proceduresStore = useProceduresStore();
const procedures = computed(() => proceduresStore.procedures);

onMounted(() => {
  if ((props.routine?.origin || props.routine?.termini) && proceduresStore.procedures.length > 0) {
    executeProcedureUpdate(props.source);
  }
});

watch(
  () => proceduresStore.procedures.length,
  (newValue) => {
    if (newValue > 0 && (props.routine?.origin || props.routine?.termini)) {
      executeProcedureUpdate(props.source);
    }
  }
);

function executeProcedureUpdate(src: string) {
  const origin = props.routine?.origin;
  const termini = props.routine?.termini;

  const source = src === 'termination' ? termini : origin;
  if (!source) {
    console.error('Origin/Termini or Procedure List is missing.');
    return; // Exit early if data is incomplete
  }
  procedureFromSteps.length = 0;

  source.forEach((item, index) => {
    if (item?.procedureId) {
      procedureFromSteps.push({
        step: index + 1,
        selectedProcedure: item.procedureId,
        selectedProperty: item.propertyId || 'any',
        selectedValue: item.propertyValue || '',
        anySelectedValue: item.propertyValue ? false : true,
      });
    } else {
      console.error(`Procedure ID not found for propertyId: ${item?.propertyId}`);
    }
  });
}

function toggleAnySelectedValue(index: number) {
  procedureFromSteps[index].anySelectedValue = !procedureFromSteps[index].anySelectedValue;
}

// From Step list
const procedureFromSteps = reactive([
  {
    step: 1,
    selectedProcedure: '',
    selectedProperty: '',
    selectedValue: '',
    anySelectedValue: false,
  },
]);

//  Add new procedure step
const addProcedure = () => {
  procedureFromSteps.push({
    step: procedureFromSteps.length + 1,
    selectedProcedure: '',
    selectedProperty: '',
    selectedValue: '',
    anySelectedValue: false,
  });
};

// Remove a procedure step
const removeProcedure = (index: number) => {
  procedureFromSteps.splice(index, 1);
};

// Procedure options
const procedureList = computed(() =>
  procedures.value
  .filter((pro) => pro.archived === false)
  .map((p) => ({
    value: p.id,
    name: p.name!,
  }))
);
// Selected Procedure Property options
const getProcedurePropertyList = (index: number) => {
  const selectedProcedure = procedures.value.find(
    (p) => p.id === procedureFromSteps[index].selectedProcedure
  );
  return selectedProcedure
    ? [
        ...(selectedProcedure.fields?.filter((pf) => pf?.archived === false)
        .map((f) => ({
          value: f?.id || '',
          name: f?.name || '',
        })) || []),
        { value: 'any', name: '(Any)' },
      ]
    : [];
};

const getInputType = (index: number) => {
  const selectedProcedure = procedures.value.find(
    (p) => p.id === procedureFromSteps[index].selectedProcedure
  );
  if (!selectedProcedure) return [];

  const selectedField = selectedProcedure.fields?.find(
    (f) => f?.id === procedureFromSteps[index].selectedProperty
  );

  if (selectedField?.type === ProcedureFieldType.List) {
    if (selectedField.settings?.includes(ProcedureFieldSetting.MultiSelect)) {
      return 'multi-select';
    } else {
      return 'select';
    }
  } else if (selectedField?.type === ProcedureFieldType.Text) {
    return 'text';
  } else if (selectedField?.type === ProcedureFieldType.Number) {
    return 'number';
  } else if (selectedField?.type === ProcedureFieldType.Toggle) {
    return 'toggle';
  }

  return null;
};
// Selected Procedure Property options
const getProcedurePropertyOptionsList = (index: number) => {
  const selectedProcedure = procedures.value.find(
    (p) => p.id === procedureFromSteps[index].selectedProcedure
  );
  if (!selectedProcedure) return [];
  const selectedField = selectedProcedure.fields?.find(
    (f) => f?.id === procedureFromSteps[index].selectedProperty
  );
  return selectedField
    ? selectedField.options?.filter((pfo) => pfo?.archived === false)
    .map((o) => ({
        value: o?.id || '',
        name: o?.text || '',
      })) || []
    : [];
};

// Function to reset selected property and value
const changedSelectedProcedure = (index: number) => {
  procedureFromSteps[index].selectedProperty = 'any';
  procedureFromSteps[index].selectedValue = '';
};

const chnagedSelectedProperty = (index: number) => {
  if (getInputType(index) === 'toggle') {
    procedureFromSteps[index].selectedValue = 'off';
  } else {
    procedureFromSteps[index].selectedValue = '';
  }
};

// Method to get the toggle value based on index
function getToggleValue(index: number): boolean {
  return procedureFromSteps[index].selectedValue === 'on';
}

// Method to update the toggle value based on index
function updateToggleValue(index: number, value: boolean) {
  procedureFromSteps[index].selectedValue = value ? 'on' : 'off';
}
function getMultiSelectValue(index: number) {
  if (!procedureFromSteps[index].selectedValue) return [];
  // Split the comma-separated IDs and find corresponding option objects
  const selectedIds = procedureFromSteps[index].selectedValue.split(',');
  return getProcedurePropertyOptionsList(index)?.filter((option) =>
    selectedIds.includes(option.value)
  );
}

function updateMultiSelectValue(index: number, newValue: { name: string; value: string }[]) {
  // Convert the array of selected objects into a comma-separated string of IDs
  procedureFromSteps[index].selectedValue = newValue.map((option) => option.value).join(',');
}

function getNumberValue(index: number) {
  return procedureFromSteps[index].selectedValue;
}

function updateNumberValue(index: number, value: number) {
  procedureFromSteps[index].selectedValue = value.toString();
}

// Validate from step
const completeProcedureFormSteps = computed(() => {
  const lastProcedureFormStep = procedureFromSteps[procedureFromSteps.length - 1];
  return !!(
    lastProcedureFormStep.selectedProcedure &&
    (lastProcedureFormStep.selectedProperty === 'any' ||
      lastProcedureFormStep.selectedValue ||
      lastProcedureFormStep.anySelectedValue)
  );
});

watch(completeProcedureFormSteps, (newValue) => {
  emit('complete-setp', newValue);
});
watch(procedureFromSteps, () => {
  emit('trigger-updated', procedureFromSteps);
});
</script>
<template>
  <div class="pl-4 mt-2 ml-8 border-l-[1px] border-slate-300">
    <div v-for="(procedurFromStep, index) in procedureFromSteps" :key="index">
      <!-- remove procedure button -->
      <div v-if="index > 0" class="my-8">
        <button
          @click="removeProcedure(index)"
          class="flex gap-2 items-center text-slate-900 text-base font-semibold"
        >
          <IconMinusCircle color="#F43E5C" /> or
        </button>
      </div>

      <div class="flex gap-3">
        <!-- Counter -->
        <span
          class="p-1 w-[40px] h-[40px] flex items-center justify-center text-xs leading-[18px] font-semibold rounded bg-slate-100"
          >{{ index + 1 }}</span
        >
        <div class="flex-1 flex flex-col gap-4">
          <FwbSelect
            v-if="procedurFromStep.step >= 1"
            v-model="procedurFromStep.selectedProcedure"
            :options="procedureList"
            @change="changedSelectedProcedure(index)"
            placeholder="Select Procedure"
          />
          <FwbSelect
            v-if="procedurFromStep.selectedProcedure"
            v-model="procedurFromStep.selectedProperty"
            :options="getProcedurePropertyList(index)"
            @change="chnagedSelectedProperty(index)"
            class="flex-1"
            placeholder="Select Procedure Property"
          />
          <!-- Render Inputs Based on Field Type -->
          <div v-if="procedurFromStep.selectedProperty">
            <!-- Multi-Select for field.type === 'LIST' and field.settings includes 'MULTI_SELECT' -->
            <multiselect
              v-if="getInputType(index) === 'multi-select'"
              :modelValue="getMultiSelectValue(index)"
              :options="getProcedurePropertyOptionsList(index)"
              @update:modelValue="updateMultiSelectValue(index, $event)"
              placeholder="Select Values"
              :multiple="true"
              :close-on-select="false"
              track-by="value"
              :searchable="false"
              label="name"
              :disabled="procedurFromStep.anySelectedValue"
            />
            <!-- Single Select for field.type === 'LIST' without 'MULTI_SELECT' -->
            <FwbSelect
              v-if="getInputType(index) === 'select'"
              v-model="procedurFromStep.selectedValue"
              :options="getProcedurePropertyOptionsList(index)"
              placeholder="Select Value"
              :disabled="procedurFromStep.anySelectedValue"
            />

            <!-- Text Input for field.type === 'TEXT' -->
            <fwb-input
              v-if="getInputType(index) === 'text'"
              v-model="procedurFromStep.selectedValue"
              type="text"
              placeholder="Enter Text"
              :disabled="procedurFromStep.anySelectedValue"
            />

            <!-- Number Input for field.type === 'NUMBER' -->
            <fwb-input
              v-if="getInputType(index) === 'number'"
              :modelValue="getNumberValue(index)"
              @update:modelValue="updateNumberValue(index, $event)"
              type="number"
              placeholder="Enter Number"
              :disabled="procedurFromStep.anySelectedValue"
            />

            <!-- Toggle Input for field.type === 'TOGGLE' -->
            <fwb-toggle
              v-if="getInputType(index) === 'toggle'"
              v-bind:model-value="getToggleValue(index)"
              @update:modelValue="updateToggleValue(index, $event)"
              :disabled="procedurFromStep.anySelectedValue"
              :class="{ 'cursor-not-allowed': procedurFromStep.anySelectedValue }"
            />
            <!-- Toggle Input for any -->
            <div v-if="procedurFromStep.selectedProperty !== 'any'" class="mt-3">
              <fwb-toggle
                :model-value="procedurFromStep.anySelectedValue"
                @change="toggleAnySelectedValue(index)"
                label="(Any)"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Add procedure button -->
    <div v-if="completeProcedureFormSteps" class="mt-8">
      <button
        @click="addProcedure"
        class="flex gap-2 items-center text-brand-600 text-sm font-medium"
      >
        <IconPlusCircle /> Add Procedure
      </button>
    </div>
  </div>
</template>
