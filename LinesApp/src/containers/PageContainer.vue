<template>
    <div
        :class="containerClass"
        :style="containerStyle"
    >
        <slot></slot>
    </div>
</template>

<script>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

export default {
    setup() {
        const route = useRoute();

        // Define computed style based on route path
        const containerStyle = computed(() => {
            return route.path === '/reporting' ? { padding: '0' } : {};
        });

        // Optional: add additional classes if needed
        const containerClass = 'p-4 mb-16 lg:mb-0 lg:p-10 overflow-auto max-h-[calc(100vh-185px)] lg:max-h-[calc(100vh-144px)]';

        return {
            containerClass,
            containerStyle,
        };
    }
};
</script>

<style scoped>
</style>
