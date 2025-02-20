<script setup lang="ts">
import { FwbButton } from 'flowbite-vue';
import { Dropdown } from '@/components/dropdown/index';
import { computed } from 'vue';
import { IconDotHorizontal } from '@/components/icons/index';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { useUsersStore } from '@/stores/data/settings/users';
import { MedicalRecord, User } from '@/api/__generated__/graphql';
import UserAvatar from '@/components/avatar/UserAvatar.vue';

const props = defineProps<{
  record: MedicalRecord;
}>();

const jobsStore = useJobsStore();
const usersStore = useUsersStore();

const observations = computed(() => {
  const matchingNotes = jobsStore.jobs
    .flatMap((job) => job.notes)
    .filter((note) =>
      props.record.observations?.some((observation) => observation?.objectId === note?.id)
    );

  return matchingNotes.sort((a: any, b: any) => {
    return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
  });
});

const formatDate = (date: Date): string => {
  const options: Intl.DateTimeFormatOptions = {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
    hour: 'numeric',
    minute: 'numeric',
    hour12: true,
  };
  return new Intl.DateTimeFormat('en-US', options).format(date);
};

function getUserName(userId: string): string {
  const user = usersStore.users.find((user) => user.id === userId);
  if(user){
    return user.firstName + ' ' + user.lastName;
  }
  return '';
}
function getUserForAvatar(id: string) {
  return usersStore.users.find((user) => user.id === id) as User;
}
</script>

<template>
  <div>
    <!-- observations -->
    <template v-if="observations">
      <div
        v-for="(observation, index) in observations"
        :key="index"
        class="flex border-b border-slate-300 pb-6 mb-6"
      >
        <user-avatar
          :user="getUserForAvatar(observation?.createdBy)"
          rounded
          size="sm"
          class="mr-3"
        />
        <div class="flex flex-col flex-1">
          <div class="flex items-start justify-between">
            <div>
              <div class="text-slate-500 font-medium text-xs">
                {{ formatDate(new Date(observation?.createdAt)) }}
              </div>
              <div class="text-slate-900 font-semibold text-sm">
                {{ getUserName(observation?.createdBy) }}
              </div>
            </div>
            <div class="flex items-center gap-1">
              <Dropdown align-to-end class="rounded-lg" close-inside>
                <template #trigger>
                  <fwb-button color="light" class="border-white focus:ring-0" pill square size="sm">
                    <IconDotHorizontal />
                  </fwb-button>
                </template>
                <!-- <DropdownMenu class="min-w-36 max-w-36">
                  <DropdownItem>
                    <IconTrash01 class="mr-2" />
                    Delete
                  </DropdownItem>
                </DropdownMenu> -->
              </Dropdown>
            </div>
          </div>
          <div class="font-medium text-sm pt-2 text-gray-900">
            {{ observation?.text }}
          </div>
        </div>
      </div>
    </template>
  </div>
</template>
