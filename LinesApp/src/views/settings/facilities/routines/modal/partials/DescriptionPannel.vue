<script setup lang="ts">
import AccordionDefault from '@/components/accordion/AccordionDefault.vue';
import {
  IconEdit03
} from '@/components/icons/index';
import {  FwbButton, FwbTextarea } from 'flowbite-vue';
import { ref, onMounted, watch } from 'vue';

const props = defineProps<{ description: string }>();
const emit = defineEmits(['description-updated']);

const descriptionEdit = ref<string>('');


const description = ref(``);
const isEditDescription = ref(false);

onMounted(() => {
  description.value = props.description;
});
watch(
  () => props.description,
  (newValue) => {
    description.value = newValue;
  }
);


function handleDescriptionEditIcon() {
  isEditDescription.value = true;
  descriptionEdit.value = description.value;
}

function handleEditDescription() {
    description.value = descriptionEdit.value;
    isEditDescription.value = false;
    emit('description-updated', description.value);
    // console.log('val des',description.value);
}
</script>

<template>
  <!-- description  -->
  <AccordionDefault id="1" active class="pb-2 lg:pb-4 border-slate-300">
    <template #header>
      <div class="w-full flex items-center justify-between">
        <div>Description</div>

        <fwb-button
          v-if="!isEditDescription"
          @click.stop="handleDescriptionEditIcon"
          color="light"
          pill
          square
          class="border-transparent"
        >
          <IconEdit03 />
        </fwb-button>
      </div>
    </template>

    <div v-if="!isEditDescription" class="text-xs leading-[18px] font-medium text-slate-500">
      {{ description }}
    </div>

    <!-- edit description -->
    <div v-if="isEditDescription" class="flex flex-col gap-4 pt-2">
      <div>
        <label class="mb-2 block text-sm font-medium text-gray-900 dark:text-white"
          >Description</label
        >
        <fwb-textarea
          v-model="descriptionEdit"
          :rows="6"
          label=""
          />
      </div>

      <div class="flex gap-4 items-center justify-end">
        <fwb-button @click="isEditDescription = false" color="light" pill>Cancel</fwb-button>
        <fwb-button @click="handleEditDescription" class="bg-primary-600 hover:bg-brand-600" pill
          >Confirm</fwb-button
        >
      </div>
    </div>
  </AccordionDefault>
</template>


